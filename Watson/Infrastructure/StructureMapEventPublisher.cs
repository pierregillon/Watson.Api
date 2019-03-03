using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        public async Task Publish(IEnumerable<IEvent> events)
        {
            var handlesByType = events
                .GroupBy(x=>x.GetType())
                .Select(x=> new {
                    Type = x.Key,
                    Handles = GetHandleMethods(x.Key)
                })
                .ToDictionary(x=>x.Type);
           
            await Task.Factory.StartNew(async () => {
                foreach (var @event in events) {
                    foreach (var handle in handlesByType[@event.GetType()].Handles) {
                        await handle(@event);
                    }
                }
            });
        }

        private IEnumerable<Func<IEvent, Task>> GetHandleMethods(Type type) {
            var genericListenerType = typeof(IEventHandler<>);
            var listenerType = genericListenerType.MakeGenericType(type);
            var listeners = _container.GetAllInstances(listenerType);
            var handleMethods = new List<Func<IEvent, Task>>();
            foreach (var listener in listeners){
                var handleMethod = listener.GetType().GetMethod("Handle");
                handleMethods.Add(@event => {
                    return (Task)handleMethod.Invoke(listener, new object[] { @event });
                });
            }
            return handleMethods;
        }
    }
}