using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Watson.Infrastructure.Logging
{
    public class LoggerBroadcaster : ILogger
    {
        private readonly IEnumerable<ILogger> _loggers;

        public LoggerBroadcaster(params ILogger[] loggers)
        {
            _loggers = loggers;
        }

        public Task Log(MonitoringLogEntry data)
        {
            return Task.WhenAll(_loggers.Select(x => x.Log(data)).ToArray());
        }
        public Task Log(ErrorLogEntry data)
        {
            return Task.WhenAll(_loggers.Select(x => x.Log(data)).ToArray());
        }
    }
}