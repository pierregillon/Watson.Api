using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CQRSlite.Events;
using EventStore.ClientAPI;
using Newtonsoft.Json;

namespace Watson.Infrastructure
{
    public class EventStore : IEventStore
    {
        private const int EVENT_COUNT = 200;
        private IEventStoreConnection _connection;
        private static readonly Type[] _types = Assembly.GetExecutingAssembly().GetTypes();

        public EventStore()
        {
            _connection = EventStoreConnection.Create(new Uri("tcp://admin:changeit@localhost:1113"),"InputFromFileConsoleApp");
        }

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

        public async Task<IEnumerable<IEvent>> GetAllEventsFromBegining()
        {
            var streamEvents = await ReadAllEvents();

            return streamEvents
                .Select(ConvertToDomainEvent)
                .ToArray();
        }

        private IEvent ConvertToDomainEvent(ResolvedEvent @event)
        {
            var json = Encoding.UTF8.GetString(@event.Event.Data);
            var type = _types.Single(x => x.Name == @event.Event.EventType);
            return (IEvent)JsonConvert.DeserializeObject(json, type);
        }

        public async Task Save(IEnumerable<IEvent> events, CancellationToken cancellationToken = default(CancellationToken))
        {
            foreach (var @event in events) {
                var eventData = new EventData(
                    eventId: Guid.NewGuid(), 
                    type: @event.GetType().Name,
                    isJson: true,
                    data: Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@event)),
                    metadata: null
                );

                await _connection.AppendToStreamAsync(@event.Id.ToString(), @event.Version, eventData);
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