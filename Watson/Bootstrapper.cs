using Watson.Infrastructure;
using Nancy.Bootstrappers.StructureMap;
using StructureMap;
using Nancy.Bootstrapper;
using CQRSlite.Events;
using Watson.Infrastructure.Logging;
using System;
using Microsoft.Extensions.Configuration;

namespace Watson.Server
{
    public class Bootstrapper : StructureMapNancyBootstrapper
    {
        private AppSettings settings;

        public Bootstrapper(AppSettings settings)
        {
            this.settings = settings;
        }

        protected override IContainer GetApplicationContainer()
        {
            var containerBuilder = new StructureMapContainerBuilder();
            return containerBuilder.Build(settings);
        }

        protected override async void ApplicationStartup(IContainer container, IPipelines pipelines)
        {
            var logger = container.GetInstance<ConsoleLogger>();
            var eventStore = container.GetInstance<EventStoreOrg>();
            var eventPublisher = container.GetInstance<IEventPublisher>();
            var settings = container.GetInstance<AppSettings>();

            logger.WriteLine(ConsoleColor.DarkYellow, $"* Connecting to eventstore ...");
            await eventStore.Connect(
                settings.EventStore.Server,
                settings.EventStore.Port,
                settings.EventStore.User,
                settings.EventStore.Password
            );
            logger.WriteLine(ConsoleColor.DarkYellow,$"* Reading all events from beginning ...");
            var events = await eventStore.ReadAllEventsFromBeginning();
            foreach (var @event in events) {
                await eventPublisher.Publish(@event);
            }
            logger.WriteLine(ConsoleColor.DarkYellow,$"* DONE");
        }
    }
}