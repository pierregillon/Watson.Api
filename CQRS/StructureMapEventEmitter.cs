using System;
using System.Linq;
using System.Threading.Tasks;
using StructureMap;

namespace fakenewsisor.server
{
    public class StructureMapEventEmitter : IEventEmitter
    {
        private readonly IContainer _container;

        public StructureMapEventEmitter(IContainer container)
        {
            _container = container;
        }

        public async void Emit<T>(T @event)
        {
            var listeners = _container.GetAllInstances<IEventListener<T>>();
            foreach (var handler in listeners)
            {
                await handler.On(@event);
            }
        }
    }
}