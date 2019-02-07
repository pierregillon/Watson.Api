using Watson.Infrastructure;
using Nancy.Bootstrappers.StructureMap;
using StructureMap;
using Nancy.Bootstrapper;
using CQRSlite.Events;
using Watson.Infrastructure.Logging;
using System;
using Microsoft.Extensions.Configuration;
using Nancy;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

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

            
        }
    }

    public class LoadEventsStartup : IApplicationStartup
    {
        private readonly ConsoleLogger logger;
        private readonly EventStoreOrg eventStore;
        private readonly IEventPublisher eventPublisher;
        private readonly AppSettings settings;

        public LoadEventsStartup(
            ConsoleLogger logger, 
            EventStoreOrg eventStore, 
            IEventPublisher eventPublisher,
            AppSettings settings)
        {
            this.logger = logger;
            this.eventStore = eventStore;
            this.eventPublisher = eventPublisher;
            this.settings = settings;
        }

        public async void Initialize(IPipelines pipelines)
        {
            pipelines.BeforeRequest.InsertItemAtPipelineIndex(0, new PipelineItem<Func<NancyContext, Response>>("LoadDomainEvents", InterceptRequests), false);

            try
            {
                await ReadAndPublishAllEvents();
            }
            finally
            {
                pipelines.BeforeRequest.RemoveByName("LoadDomainEvents");
            }
        }

        private async Task ReadAndPublishAllEvents()
        {
            logger.WriteLine(ConsoleColor.DarkYellow, $"* Connecting to eventstore ...");
            await eventStore.Connect(
                settings.EventStore.Server,
                settings.EventStore.Port,
                settings.EventStore.User,
                settings.EventStore.Password
            );
            
            logger.WriteLine(ConsoleColor.DarkYellow, $"* Reading all events from beginning ...");
            var events = await eventStore.ReadAllEventsFromBeginning();
            
            logger.WriteLine(ConsoleColor.DarkYellow, $"* Publishing {events.Count()} events ...");
            foreach (var @event in events) {
                await eventPublisher.Publish(@event);
            }
            
            logger.WriteLine(ConsoleColor.DarkYellow,$"* DONE");
        }

        private Response InterceptRequests(NancyContext context)
        {
            return new Nancy.Response() {
                StatusCode = HttpStatusCode.Processing,
                ContentType = "text/plain",
                Contents = x => {
                    var bytes = UTF8Encoding.UTF8.GetBytes("Server loading");
                    x.Write(bytes, 0, bytes.Length);
                }
            };
        }
    }
}