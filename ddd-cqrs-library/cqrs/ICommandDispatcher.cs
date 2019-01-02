using System.Threading.Tasks;

namespace try4real.cqrs
{
    public interface ICommandDispatcher
    {
        Task Dispatch<T>(T command);
        Task<TResult> Dispatch<TCommand, TResult>(TCommand command);
    }
}