using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fakenewsisor.server.DDD_CQRS;

namespace fakenewsisor.server
{
    public class WebPageFinder : IEventListener<WebPageRegistered>
    {
        private readonly InMemoryDatabase database;

        public WebPageFinder(InMemoryDatabase database)
        {
            this.database = database;
        }

        public void On(WebPageRegistered @event)
        {
            database.Table<WebPageListItem>()
                    .Add(new WebPageListItem
                    {
                        Id = @event.Id,
                        Url = @event.Url
                    });
        }

        public WebPageListItem SearchByUrl(string webPageUrl)
        {
            return database.Table<WebPageListItem>().FirstOrDefault(x => x.Url == webPageUrl);
        }
    }
}