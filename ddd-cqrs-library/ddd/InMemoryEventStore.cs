using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace try4real.ddd
{
    public class InMemoryEventStore : IEventStore
    {
        private readonly IEventPublisher _publisher;
        private readonly IDictionary<Guid, List<EventDescriptor>> _current = new ConcurrentDictionary<Guid, List<EventDescriptor>>();

        public InMemoryEventStore(IEventPublisher publisher)
        {
            _publisher = publisher;
        }

        public async Task SaveEvents(Guid aggregateId, IEnumerable<Event> events, int expectedVersion)
        {
            await Task.Delay(0);

            List<EventDescriptor> eventDescriptors;

            if (!_current.TryGetValue(aggregateId, out eventDescriptors))
            {
                eventDescriptors = new List<EventDescriptor>();
                _current.Add(aggregateId, eventDescriptors);
            }
            else if (IsEventVersionValid(expectedVersion, eventDescriptors))
            {
                throw new ConcurrencyException();
            }

            var i = expectedVersion;

            foreach (var @event in events)
            {
                i++;

                @event.Version = i;
                eventDescriptors.Add(new EventDescriptor(aggregateId, @event, i));
                _publisher.Publish(@event);
            }
        }

        public async Task<IEnumerable<Event>> GetEventsForAggregate(Guid aggregateId)
        {
            await Task.Delay(0);

            List<EventDescriptor> eventDescriptors;

            if (!_current.TryGetValue(aggregateId, out eventDescriptors))
            {
                throw new AggregateNotFound();
            }

            return eventDescriptors.Select(desc => desc.EventData).ToList();
        }

        private static bool IsEventVersionValid(int expectedVersion, IList<EventDescriptor> eventDescriptors)
        {
            return eventDescriptors[eventDescriptors.Count - 1].Version != expectedVersion && expectedVersion != -1;
        }

        // ----- Private structure
        private struct EventDescriptor
        {
            public readonly Event EventData;
            public readonly Guid Id;
            public readonly int Version;

            public EventDescriptor(Guid id, Event eventData, int version)
            {
                EventData = eventData;
                Version = version;
                Id = id;
            }
        }
    }

    public class AggregateNotFound : Exception
    {
    }

    public class ConcurrencyException : Exception
    {
    }
}