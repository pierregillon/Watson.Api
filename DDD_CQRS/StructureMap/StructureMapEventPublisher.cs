using System.Linq;
using System.Threading;
using fakenewsisor.server.DDD_CQRS;
using StructureMap;

namespace fakenewsisor.server.DDD_CQRS.StructureMap
{
    public class StructureMapEventPublisher : IEventPublisher
    {
        private readonly IContainer _container;

        public StructureMapEventPublisher(IContainer container)
        {
            this._container = container;
        }

        public void Publish(Event @event)
        {
            var genericListenerType = typeof(IEventListener<>);
            var listenerType = genericListenerType.MakeGenericType(@event.GetType());
            var onMethod = listenerType.GetMethod("On");
            var listeners = _container.GetAllInstances(listenerType);
            ThreadPool.QueueUserWorkItem(x =>
            {
                foreach (var handler in listeners)
                {
                    onMethod.Invoke(handler, new object[] { @event });
                }
            });

        }
    }
}