using System;
using System.Threading.Tasks;

namespace try4real.ddd
{
    public interface IRepository<T> where T : AggregateRoot, new()
    {
        Task<T> Load(Guid id);
        Task Save(AggregateRoot aggregate);
    }
}