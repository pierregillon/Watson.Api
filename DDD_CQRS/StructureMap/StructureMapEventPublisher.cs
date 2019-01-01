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

        public void Publish<T>(T @event) where T : Event
        {
            var listeners = _container.GetAllInstances<IEventListener<T>>();
            ThreadPool.QueueUserWorkItem(x =>
            {
                foreach (var handler in listeners)
                {
                    handler.On(@event);
                }
            });
        }
    }
}