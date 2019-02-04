using System;
using System.Diagnostics;
using System.Collections.Generic;
using Nancy;

namespace Watson.Infrastructure.Logging
{
    public class MonitoringLogEntry : LogEntry
    {
        private readonly Stopwatch _watch;
        
        public long ExecutionTimeMs { get; set; }

        public MonitoringLogEntry(NancyContext context)
        {
            User = context.CurrentUser?.Identity.Name ?? "anonymous";
            RequestProtocolVersion = context.Request.ProtocolVersion;
            RequestPath = context.Request.Path;
            RequestMethod = context.Request.Method;
            RequestBody = context.Request.Form.ToDictionary();
            RequestQuery = context.Request.Query.ToDictionary();
            
            _watch = Stopwatch.StartNew();
        }

        public void Stop()
        {
            _watch.Stop();
            ExecutionTimeMs = _watch.ElapsedMilliseconds;
        }
    }
}   