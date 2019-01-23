using Watson.Infrastructure;
using Nancy.Bootstrappers.StructureMap;
using StructureMap;
using Nancy.Bootstrapper;
using CQRSlite.Events;

namespace Watson.Server
{
    public class Bootstrapper : StructureMapNancyBootstrapper
    {
        protected override IContainer GetApplicationContainer()
        {
            var containerBuilder = new StructureMapContainerBuilder();
            return containerBuilder.Build();
        }

        protected override async void ApplicationStartup(IContainer container, IPipelines pipelines)
        {
            var eventStore = container.GetInstance<EventStoreOrg>();
            var eventPublisher = container.GetInstance<IEventPublisher>();
            await eventStore.Connect();
            var events = await eventStore.ReadAllEventsFromBeginning();
            foreach (var @event in events) {
                await eventPublisher.Publish(@event);
            }
        }
    }
}