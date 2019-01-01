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
            if (handlers.Count() > 1)
            {
                throw new Exception("Only 1 handler required.");
            }
            await handlers.Single().Handle(command);
        }

        public async Task<TResult> Dispatch<TCommand, TResult>(TCommand command)
        {
            var handlers = _container.GetAllInstances<ICommandHandler<TCommand, TResult>>();
            if (handlers.Any() == false)
            {
                throw new Exception("No handlers found");
            }
            if (handlers.Count() > 1)
            {
                throw new Exception("Only 1 handler required.");
            }
            return await handlers.Single().Handle(command);
        }
    }
}