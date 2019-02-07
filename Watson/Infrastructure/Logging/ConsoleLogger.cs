using System;
using System.Linq;
using System.Threading.Tasks;
using Watson.Infrastructure.Logging;

namespace Watson.Infrastructure
{
    public class ConsoleLogger : ILogger
    {
        private const ConsoleColor INFO_COLOR = ConsoleColor.DarkBlue;
        private const ConsoleColor ERROR_COLOR = ConsoleColor.Red;

        public Task Log(MonitoringLogEntry data)
        {
            WriteLine(INFO_COLOR, $"[{data.Time}] {data.RequestProtocolVersion} {data.RequestMethod} {data.RequestPath} => {data.ExecutionTimeMs} ms");
            return Task.Delay(0);
        }

        public Task Log(ErrorLogEntry data)
        {
            WriteLine(ERROR_COLOR, $"[{data.Time}] {data.RequestMethod} {data.RequestPath} : {data.ErrorMessage}\r\n----------\r\n{data.ErrorStackTrace}");
            return Task.Delay(0);
        }

        public void WriteLine(ConsoleColor color, string message)
        {
            var beforeColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = beforeColor;
        }
    }
}