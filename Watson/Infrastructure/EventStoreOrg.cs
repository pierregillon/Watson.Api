using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CQRSlite.Events;
using EventStore.ClientAPI;
using Newtonsoft.Json;

namespace Watson.Infrastructure
{
    public class EventStoreOrg : IEventStore
    {
        private const int EVENT_COUNT = 200;

        private IEventStoreConnection _connection;
        private readonly JsonSerializerSettings _serializerSettings;
        private readonly ILogger _logger;
        private readonly ITypeLocator _typeLocator;

        public EventStoreOrg(ILogger logger, ITypeLocator typeLocator)
        {
            _logger = logger;
            _typeLocator = typeLocator;

            var jsonResolver = new PropertyCleanerSerializerContractResolver();
            jsonResolver.IgnoreProperty(typeof(IEvent), "Version", "TimeStamp");
            jsonResolver.RenameProperty(typeof(IEvent), "Id", "AggregateId");

            _serializerSettings = new JsonSerializerSettings {
                ContractResolver = jsonResolver,
                Formatting = Formatting.Indented
            };
        }

        // ----- Public methods

        public async Task Connect(string server, int port = 1113, string login = "admin", string password = "changeit")
        {
            _connection = EventStoreConnection.Create(new Uri($"tcp://{login}:{password}@{server}:{port}"), "Watson.Api");
            await _connection.ConnectAsync();
        }

        public async Task<IEnumerable<IEvent>> Get(Guid aggregateId, int fromVersion, CancellationToken cancellationToken = default(CancellationToken))
        {
            var streamEvents = await ReadAllEventsInStream(aggregateId.ToString());
            
            return streamEvents
                .Select(ConvertToDomainEvent)
                .ToArray();
        }

        public async Task<IEnumerable<IEvent>> ReadAllEventsFromBeginning()
        {
            var streamEvents = await ReadAllEvents();

            return streamEvents
                .Where(x => x.OriginalStreamId.StartsWith("$") == false)
                .Select(TryConvertToDomainEvent)
                .Where(x => x != null)
                .ToArray();
        }
        
        public async Task Save(IEnumerable<IEvent> events, CancellationToken cancellationToken = default(CancellationToken))
        {
            foreach (var @event in events) {
                var json = JsonConvert.SerializeObject(@event, _serializerSettings);
                var eventData = new EventData(
                    eventId: Guid.NewGuid(), 
                    type: @event.GetType().Name,
                    isJson: true,
                    data: Encoding.UTF8.GetBytes(json),
                    metadata: null
                );
                var version = @event.Version == 1 ? ExpectedVersion.NoStream : @event.Version;
                await _connection.AppendToStreamAsync(@event.Id.ToString(), version, eventData);
            }
        }

        // ----- Internal logics

        private IEvent ConvertToDomainEvent(ResolvedEvent @event)
        {
            var json = Encoding.UTF8.GetString(@event.Event.Data);
            var type = _typeLocator.Find(@event.Event.EventType);
            if (type == null) {
                throw new UnknownEvent(@event.Event.EventType);
            }
            var domainEvent = (IEvent)JsonConvert.DeserializeObject(json, type, _serializerSettings);
            domainEvent.Version = (int)@event.OriginalEventNumber;
            return (IEvent)domainEvent;
        }

        private IEvent TryConvertToDomainEvent(ResolvedEvent @event)
        {
            try
            {
                return ConvertToDomainEvent(@event);
            }
            catch (UnknownEvent ex)
            {
                _logger.Error(ex, null);
                return null;
            }
        }

        private async Task<IEnumerable<ResolvedEvent>> ReadAllEventsInStream(string streamId)
        {
            var streamEvents = new List<ResolvedEvent>();
            StreamEventsSlice currentSlice;
            var nextSliceStart = (long)StreamPosition.Start;
            
            do {
                currentSlice = await _connection.ReadStreamEventsForwardAsync(streamId, nextSliceStart, EVENT_COUNT, false);
                nextSliceStart = currentSlice.NextEventNumber;
                streamEvents.AddRange(currentSlice.Events);
            } while (!currentSlice.IsEndOfStream);

            return streamEvents;
        }

        private async Task<IEnumerable<ResolvedEvent>> ReadAllEvents()
        {
            var streamEvents = new List<ResolvedEvent>();
            AllEventsSlice currentSlice;
            var nextSliceStart = Position.Start;
            
            do {
                currentSlice = await _connection.ReadAllEventsForwardAsync(nextSliceStart, EVENT_COUNT, false);
                nextSliceStart = currentSlice.NextPosition;
                streamEvents.AddRange(currentSlice.Events);
            } while (!currentSlice.IsEndOfStream);

            return streamEvents;
        }
    }
}