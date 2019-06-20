using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRSlite.Events;
using StructureMap;
using Watson.Domain._Infrastructure;

namespace Watson.Infrastructure
{
    public class StructureMapEventProjectionFeeder : IEventProjectionFeeder
    {
        private readonly IContainer _container;

        public StructureMapEventProjectionFeeder(IContainer container)
        {
            _container = container;

        }

        public async Task Feed(IEvent @event)
        {
            var genericListenerType = typeof(IEventHandler<>);
            var listenerType = genericListenerType.MakeGenericType(@event.GetType());
            var listeners = _container.GetAllInstances(listenerType);

            foreach (var handler in listeners) {
                var onMethod = handler.GetType().GetMethod("Handle");
                await (Task)onMethod.Invoke(handler, new object[] { @event });
            }

            await Task.Delay(0);
        }

        public async Task Feed(IEnumerable<IEvent> events)
        {
            var handlesByType = events
                .GroupBy(x => x.GetType())
                .Select(x => new {
                    Type = x.Key,
                    Handles = GetHandleMethods(x.Key)
                })
                .ToDictionary(x => x.Type);

            await Task.Factory.StartNew(async () => {
                foreach (var @event in events) {
                    foreach (var handle in handlesByType[@event.GetType()].Handles) {
                        await handle(@event);
                    }
                }
            });
        }

        private IEnumerable<Func<IEvent, Task>> GetHandleMethods(Type type)
        {
            var genericListenerType = typeof(IEventHandler<>);
            var listenerType = genericListenerType.MakeGenericType(type);
            var listeners = _container.GetAllInstances(listenerType);
            var handleMethods = new List<Func<IEvent, Task>>();
            foreach (var listener in listeners) {
                var handleMethod = listener.GetType().GetMethod("Handle");
                handleMethods.Add(@event => 
                    (Task)handleMethod.Invoke(listener, new object[] { @event }));
            }
            return handleMethods;
        }
    }
}