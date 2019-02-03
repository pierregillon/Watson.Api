using System.Threading.Tasks;

namespace Watson.Infrastructure.Logging
{
    public interface ILogger
    {
        Task Log(MonitoringLogEntry data);
        Task Log(ErrorLogEntry data);
    }
}