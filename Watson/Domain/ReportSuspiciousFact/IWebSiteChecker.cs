using System.Threading.Tasks;

namespace Watson.Domain.ReportSuspiciousFact
{
    public interface IWebSiteChecker
    {
        Task<bool> IsOnline(string documentUrl);
    }
}