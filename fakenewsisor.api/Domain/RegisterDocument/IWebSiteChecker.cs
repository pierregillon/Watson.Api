using System.Threading.Tasks;

namespace fakenewsisor.server
{
    public interface IWebSiteChecker
    {
        Task CheckReachable(string documentUrl);
    }
}