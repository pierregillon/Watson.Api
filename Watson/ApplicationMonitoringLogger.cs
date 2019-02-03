using Nancy.Bootstrapper;
using System;
using System.Diagnostics;
using EventStore.ClientAPI;
using Nancy;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Watson.Server
{
    public class ApplicationMonitoringLogger : IApplicationStartup
    {
        private readonly ILogger logger;
        private readonly IDictionary<Request, MonitorEntry> _currentMonitoring = new ConcurrentDictionary<Request, MonitorEntry>();

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
            var monitorEntry = new MonitorEntry {
                User = context.CurrentUser?.Identity.Name ?? "anonymous",
                RequestProtocolVersion = context.Request.ProtocolVersion,
                RequestPath = context.Request.Path,
                RequestMethod = context.Request.Method,
                RequestBody = context.Request.Form,
                RequestQuery = context.Request.Query,
            };
            _currentMonitoring.Add(context.Request, monitorEntry);
        }

        private async Task EndMonitoring(NancyContext context) {
            if (_currentMonitoring.TryGetValue(context.Request, out MonitorEntry monitorEntry)) {
                try {
                    monitorEntry.Stop();
                    logger.Debug(JsonConvert.SerializeObject(monitorEntry));
                }
                finally {
                    _currentMonitoring.Remove(context.Request);
                }
            }
            await Task.Delay(0);
        }

        public class MonitorEntry 
        {
            private readonly Stopwatch _watch;

            public DateTimeOffset Time { get; set; } = DateTimeOffset.UtcNow;
            public string User { get; set; }
            public string RequestProtocolVersion { get; set; }
            public string RequestPath { get; set; }
            public string RequestMethod { get; set; }
            public dynamic RequestBody { get; set; }
            public dynamic RequestQuery { get; set; }
            public long ExecutionTimeMs { get; set; }

            public MonitorEntry()
            {
                _watch = Stopwatch.StartNew();
            }

            public void Stop()
            {
                _watch.Stop();
                ExecutionTimeMs = _watch.ElapsedMilliseconds;
            }
        }
    }
}   