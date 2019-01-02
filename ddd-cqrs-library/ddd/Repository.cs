using System;
using System.Linq;
using System.Threading.Tasks;

namespace try4real.ddd
{
    public class Repository<T> : IRepository<T> where T : AggregateRoot, new()
    {
        private readonly IEventStore _storage;

        public Repository(IEventStore storage)
        {
            _storage = storage;
        }

        public async Task Save(AggregateRoot aggregate)
        {
            await _storage.SaveEvents(aggregate.Id, aggregate.GetUncommittedChanges(), aggregate.Version);
        }

        public async Task<T> Load(Guid id)
        {
            var obj = new T();
            var events = await _storage.GetEventsForAggregate(id);
            obj.LoadsFromHistory(events);
            obj.Version = events.Last().Version;
            return obj;
        }
    }
}