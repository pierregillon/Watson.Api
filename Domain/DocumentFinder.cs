using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fakenewsisor.server.Infrastructure;
using try4real.ddd;

namespace fakenewsisor.server
{
    public class DocumentFinder : IEventListener<DocumentRegistered>
    {
        private readonly InMemoryDatabase database;

        public DocumentFinder(InMemoryDatabase database)
        {
            this.database = database;
        }

        public void On(DocumentRegistered @event)
        {
            database.Table<DocumentListItem>()
                    .Add(new DocumentListItem
                    {
                        Id = @event.DocumentId,
                        Url = @event.DocumentUrl
                    });
        }

        public DocumentListItem SearchByUrl(string documentUrl)
        {
            return database.Table<DocumentListItem>().FirstOrDefault(x => x.Url == documentUrl);
        }
    }
}