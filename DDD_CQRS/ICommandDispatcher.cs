using System.Threading.Tasks;

namespace fakenewsisor.server.DDD_CQRS
{
    public interface ICommandDispatcher
    {
        Task Dispatch<T>(T command);
        Task<TResult> Dispatch<TCommand, TResult>(TCommand command);
    }
}