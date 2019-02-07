using Nancy.Bootstrapper;
using Newtonsoft.Json;
using Watson.Infrastructure.Logging;

namespace Watson.Api.ApplicationStartups
{
    public class ApplicationErrorLogger : IApplicationStartup
    {
        private readonly ILogger logger;

        public ApplicationErrorLogger(ILogger logger)
        {
            this.logger = logger;
        }

        public void Initialize(IPipelines pipelines)
        {
            pipelines.OnError.AddItemToStartOfPipeline((context, ex) => {
                logger.Log(new ErrorLogEntry (ex, context) {
                    Context = "global catch from nancy pipeline",
                });
                return null;
            });
        }
    }
}   