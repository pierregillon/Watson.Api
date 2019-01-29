using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        private static readonly Type[] _types = Assembly.GetExecutingAssembly().GetTypes();

        private IEventStoreConnection _connection;
        private readonly JsonSerializerSettings _serializerSettings;
        private readonly ILogger _logger;
        private readonly Uri _address;

        public EventStoreOrg(ILogger logger, string server, int port = 1113, string login = "admin", string password = "changeit")
        {
            _address = new Uri($"tcp://{login}:{password}@{server}:{port}");
            _connection = EventStoreConnection.Create(_address, "Watson.Api");

            var jsonResolver = new PropertyCleanerSerializerContractResolver();
            jsonResolver.IgnoreProperty(typeof(IEvent), "Version", "TimeStamp");
            jsonResolver.RenameProperty(typeof(IEvent), "Id", "AggregateId");

            _serializerSettings = new JsonSerializerSettings {
                ContractResolver = jsonResolver,
                Formatting = Formatting.Indented
            };
            this._logger = logger;
        }

        // ----- Public methods

        public async Task Connect()
        {
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
                .Select(ConvertToDomainEvent)
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

        public override string ToString()
        {
            return _address.ToString();
        }

        // ----- Internal logics

        private IEvent ConvertToDomainEvent(ResolvedEvent @event)
        {
            var json = Encoding.UTF8.GetString(@event.Event.Data);
            var type = _types.SingleOrDefault(x => x.Name == @event.Event.EventType);
            if (type == null) {
                _logger.Error(new UnknownEvent(@event.Event.EventType), null);
                return null;
            }
            var domainEvent = (IEvent)JsonConvert.DeserializeObject(json, type, _serializerSettings);
            domainEvent.Version = (int)@event.OriginalEventNumber;
            return (IEvent)domainEvent;
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