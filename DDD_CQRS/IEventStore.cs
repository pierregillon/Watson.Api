using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace fakenewsisor.server.DDD_CQRS
{
    public interface IEventStore
    {
        Task SaveEvents(Guid id, IEnumerable<Event> enumerable, int expectedVersion);
        Task<IEnumerable<Event>> GetEventsForAggregate(Guid id);
    }
}