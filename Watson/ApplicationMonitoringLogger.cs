using Nancy.Bootstrapper;
using Nancy;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Watson.Infrastructure.Logging;

namespace Watson.Server
{
    public class ApplicationMonitoringLogger : IApplicationStartup
    {
        private readonly ILogger logger;
        private readonly IDictionary<Request, MonitoringLogEntry> _currentMonitoring = new ConcurrentDictionary<Request, MonitoringLogEntry>();

        public ApplicationMonitoringLogger(ILogger logger)
        {
            this.logger = logger;
        }

        public void Initialize(IPipelines pipelines)
        {
            pipelines.BeforeRequest += context => {
                StartMonitoring(context);
                return null;
            };

            pipelines.AfterRequest.AddItemToStartOfPipeline(async (context, response) => {
                await EndMonitoring(context);
            });

            pipelines.OnError.AddItemToStartOfPipeline((context, ex) => {
                EndMonitoring(context).Wait();
                return null;
            });
        }

        private void StartMonitoring(NancyContext context) {
            _currentMonitoring.Add(context.Request, new MonitoringLogEntry (context) {
                Context = "api monitoring from nancy pipeline",
            });
        }

        private async Task EndMonitoring(NancyContext context) {
            if (_currentMonitoring.TryGetValue(context.Request, out MonitoringLogEntry monitorEntry)) {
                try {
                    monitorEntry.Stop();
                    await logger.Log(monitorEntry);
                }
                finally {
                    _currentMonitoring.Remove(context.Request);
                }
            }
        }
    }
}   