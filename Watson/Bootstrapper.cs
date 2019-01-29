using Watson.Infrastructure;
using Nancy.Bootstrappers.StructureMap;
using StructureMap;
using Nancy.Bootstrapper;
using CQRSlite.Events;
using System;
using System.Diagnostics;
using EventStore.ClientAPI;

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
            var logger = container.GetInstance<ILogger>();
            var eventStore = container.GetInstance<EventStoreOrg>();
            var eventPublisher = container.GetInstance<IEventPublisher>();

            logger.Info($"* Connecting to eventstore {eventStore} ...");
            await eventStore.Connect();
            logger.Info($"* Reading all events from beginning ...");
            var events = await eventStore.ReadAllEventsFromBeginning();
            foreach (var @event in events) {
                await eventPublisher.Publish(@event);
            }
            logger.Info($"* DONE");
        }
    }
}