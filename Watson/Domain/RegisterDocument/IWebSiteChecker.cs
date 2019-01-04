using System.Threading.Tasks;

namespace Watson.Domain.RegisterDocument
{
    public interface IWebSiteChecker
    {
        Task<bool> IsOnline(string documentUrl);
    }
}