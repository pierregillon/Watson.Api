using System;
using System.Linq;
using System.Threading.Tasks;
using StructureMap;

namespace fakenewsisor.server.DDD_CQRS.StructureMap
{
    public class StructureMapCommandDispatcher : ICommandDispatcher
    {
        private readonly IContainer _container;

        public StructureMapCommandDispatcher(IContainer container)
        {
            _container = container;
        }

        public async Task Dispatch<T>(T command)
        {
            var handlers = _container.GetAllInstances<ICommandHandler<T>>();
            if (handlers.Any() == false)
            {
                throw new Exception("No handlers found");
            }
            foreach (var handler in handlers)
            {
                await handler.HandleAsync(command);
            }
        }

        public async Task<TResult> Dispatch<TCommand, TResult>(TCommand command)
        {
            var handler = _container.GetInstance<ICommandHandler<TCommand, TResult>>();
            if (handler == null)
            {
                throw new Exception("No handlers found");
            }
            return await handler.Handle(command);
        }
    }
}