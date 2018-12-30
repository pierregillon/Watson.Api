using System.Threading.Tasks;

namespace fakenewsisor.server
{
    public interface IWebPageRepository
    {
        Task<WebPage> Load(string webPageUrl);
        Task Save(WebPage webPage);
    }
}