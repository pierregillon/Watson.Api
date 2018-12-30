using System.Threading.Tasks;

namespace fakenewsisor.server
{
    public interface ICommandDispatcher
    {
        Task Dispatch<T>(T command);
        Task<TResult> Dispatch<TCommand, TResult>(TCommand command);
    }
}