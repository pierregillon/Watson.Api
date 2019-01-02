using System.Threading.Tasks;

namespace fakenewsisor.server
{
    public interface IWebSiteChecker
    {
        Task<bool> IsOnline(string documentUrl);
    }
}