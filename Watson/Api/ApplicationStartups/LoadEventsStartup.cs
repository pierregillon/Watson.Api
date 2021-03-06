using Watson.Infrastructure;
using Nancy.Bootstrapper;
using CQRSlite.Events;
using Watson.Infrastructure.Logging;
using System;
using Nancy;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Watson.Server;

namespace Watson.Api.ApplicationStartups
{
    public class LoadEventsStartup : IApplicationStartup
    {
        private const string PIPELINE_NAME = "LoadDomainEvents";

        private readonly ConsoleLogger logger;
        private readonly EventStoreOrg eventStore;
        private readonly StructureMapEventPublisher eventPublisher;
        private readonly StructureMapEventProjectionFeeder projectionFeeder;
        private readonly AppSettings settings;

        public LoadEventsStartup(
            ConsoleLogger logger, 
            EventStoreOrg eventStore, 
            StructureMapEventPublisher eventPublisher,
            StructureMapEventProjectionFeeder projectionFeeder,
            AppSettings settings)
        {
            this.logger = logger;
            this.eventStore = eventStore;
            this.eventPublisher = eventPublisher;
            this.projectionFeeder = projectionFeeder;
            this.settings = settings;
        }

        public async void Initialize(IPipelines pipelines)
        {
            try
            {
                pipelines.BeforeRequest.InsertItemAtPipelineIndex(0, new PipelineItem<Func<NancyContext, Response>>(PIPELINE_NAME, InterceptRequests), false);
                logger.WriteLine(ConsoleColor.DarkYellow, $"* Server loading events ...");
                await ReadAndPublishAllEvents();
                pipelines.BeforeRequest.RemoveByName(PIPELINE_NAME);
                logger.WriteLine(ConsoleColor.DarkYellow, $"* Server ready to process requests.");
            }
            catch(Exception ex)
            {
                await logger.Log(new ErrorLogEntry(ex));
                System.Diagnostics.Process.GetCurrentProcess().Kill();
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
            
            logger.WriteLine(ConsoleColor.DarkYellow, $"* Publishing {events.Count} events ...");
            await projectionFeeder.Feed(events);
        }

        private Response InterceptRequests(NancyContext context)
        {
            return new Nancy.Response() {
                StatusCode = HttpStatusCode.ServiceUnavailable,
                ContentType = "text/plain",
                Contents = x => {
                    var bytes = UTF8Encoding.UTF8.GetBytes("Server loading");
                    x.Write(bytes, 0, bytes.Length);
                }
            };
        }
    }
}