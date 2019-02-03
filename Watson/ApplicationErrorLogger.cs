using Nancy.Bootstrapper;
using System;
using EventStore.ClientAPI;
using Newtonsoft.Json;
using System.Text;

namespace Watson.Server
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
                var errorEntry = new {
                    Time = DateTimeOffset.UtcNow,
                    Message = ex.Message,
                    StackTrace = ex.StackTrace,
                    Details = BuildErrorMessage(ex)
                };
                logger.Error(JsonConvert.SerializeObject(errorEntry));
                return null;
            });
        }

        private string BuildErrorMessage(Exception ex)
        {
            var builder = new StringBuilder();
            while(ex != null) {
                builder.Append(ex.Message);
                builder.Append(Environment.NewLine);
                builder.Append("-----------------------");
                builder.Append(Environment.NewLine);
                builder.Append(ex.StackTrace);
                builder.Append(Environment.NewLine);
                builder.Append("-----------------------");
                builder.Append(Environment.NewLine);
                ex = ex.InnerException;
            }
            return builder.ToString();
        }
    }
}   