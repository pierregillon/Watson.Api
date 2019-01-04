using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CQRSlite.Events;
using CQRSlite.Messages;
using StructureMap;

namespace Watson.Infrastructure
{
    public class StructureMapEventPublisher : IEventPublisher
    {
        private readonly IContainer _container;

        public StructureMapEventPublisher(IContainer container)
        {
            this._container = container;
        }

        public async Task Publish<T>(T @event, CancellationToken cancellationToken) where T : class, IEvent
        {
            var genericListenerType = typeof(IEventHandler<>);
            var listenerType = genericListenerType.MakeGenericType(@event.GetType());
            var listeners = _container.GetAllInstances(listenerType);

            ThreadPool.QueueUserWorkItem(async x =>
            {
                foreach (var handler in listeners)
                {
                    var onMethod = handler.GetType().GetMethod("Handle");
                    await (Task)onMethod.Invoke(handler, new object[] { @event });
                }
            });

            await Task.Delay(0);
        }
    }
}