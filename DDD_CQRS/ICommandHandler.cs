using System.Threading.Tasks;

namespace fakenewsisor.server.DDD_CQRS
{
    public interface ICommandHandler<in T>
    {
        Task Handle(T command);
    }

    public interface ICommandHandler<in TCommand, TResult>
    {
        Task<TResult> Handle(TCommand command);
    }
}