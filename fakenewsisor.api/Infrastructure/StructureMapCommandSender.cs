using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CQRSlite.Commands;
using StructureMap;

namespace fakenewsisor.server.Infrastructure
{
    public class StructureMapCommandSender : ICommandSender
    {
        private readonly IContainer _container;

        public StructureMapCommandSender(IContainer container)
        {
            _container = container;
        }

        public async Task Send<T>(T command, CancellationToken cancellationToken = default(CancellationToken)) where T : class, ICommand
        {
            var type = command.GetType();
            var handlers = _container.GetAllInstances<ICommandHandler<T>>();
            if (handlers.Any() == false)
                throw new InvalidOperationException($"No handler registered for {type.FullName}");
            if (handlers.Count() != 1)
                throw new InvalidOperationException($"Cannot send to more than one handler of {type.FullName}");
            await handlers.Single().Handle(command);
        }
    }
}