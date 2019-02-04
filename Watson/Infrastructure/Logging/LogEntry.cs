using System;
using System.Collections.Generic;

namespace Watson.Infrastructure.Logging
{
    public abstract class LogEntry
    {
        public Guid Id { get; } = Guid.NewGuid();
        public DateTimeOffset Time { get; set; } = DateTimeOffset.UtcNow;
        public string User { get; set; }
        public string RequestProtocolVersion { get; set; }
        public string RequestPath { get; set; }
        public string RequestMethod { get; set; }
        public IDictionary<string, object> RequestBody { get; set; }
        public IDictionary<string, object> RequestQuery { get; set; }
        public string Context { get; set; }
    }
}