using System;
using System.Linq;
using EventStore.ClientAPI;

namespace Watson.Infrastructure
{
    public class ConsoleLogger : ILogger
    {
        private const ConsoleColor DEBUG_COLOR = ConsoleColor.DarkYellow;
        private const ConsoleColor INFO_COLOR = ConsoleColor.DarkBlue;
        private const ConsoleColor ERROR_COLOR = ConsoleColor.Red;

        public void Debug(string format, params object[] args)
        {
            WriteLine(DEBUG_COLOR, format, args);
        }

        public void Debug(Exception ex, string format, params object[] args)
        {
            WriteLine(DEBUG_COLOR, ex.ToString());
        }

        public void Error(string format, params object[] args)
        {
            WriteLine(ERROR_COLOR, format, args);
        }

        public void Error(Exception ex, string format, params object[] args)
        {
            WriteLine(ERROR_COLOR, ex.ToString());
        }

        public void Info(string format, params object[] args)
        {
            WriteLine(INFO_COLOR, format, args);
        }

        public void Info(Exception ex, string format, params object[] args)
        {
            WriteLine(INFO_COLOR, ex.ToString(), args);
        }

        private void WriteLine(ConsoleColor color, string format, params object[] args)
        {
            var beforeColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            if (args == null || args.Any() == false)     {
                Console.WriteLine(format);
            }
            else {
                Console.WriteLine(format, args);
            }
            Console.ForegroundColor = beforeColor;
        }
    }
}