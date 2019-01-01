using System;
using System.Threading.Tasks;

namespace fakenewsisor.server.DDD_CQRS
{
    public interface IRepository<T> where T : AggregateRoot, new()
    {
        Task<T> Load(Guid id);
        Task Save(AggregateRoot aggregate);
    }
}