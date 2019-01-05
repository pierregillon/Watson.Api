using System.Threading.Tasks;

namespace Watson.Domain.SuspectFalseFact
{
    public interface IWebSiteChecker
    {
        Task<bool> IsOnline(string documentUrl);
    }
}