using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fakenewsisor.server.DDD_CQRS;

namespace fakenewsisor.server
{
    public class WebPageFinder : IEventListener<WebPageRegistered>
    {
        private readonly IList<WebPageListItem> _webPages = new List<WebPageListItem>();

        public void On(WebPageRegistered @event)
        {
            _webPages.Add(new WebPageListItem
            {
                Id = @event.Id,
                Url = @event.Url
            });
        }

        public WebPageListItem SearchByUrl(string webPageUrl)
        {
            return _webPages.FirstOrDefault(x => x.Url == webPageUrl);
        }
    }
}