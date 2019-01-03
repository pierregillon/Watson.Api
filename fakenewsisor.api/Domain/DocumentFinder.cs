using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRSlite.Events;
using fakenewsisor.server.Infrastructure;

namespace fakenewsisor.server
{
    public class DocumentFinder : IEventHandler<DocumentRegistered>
    {
        private readonly InMemoryDatabase database;

        public DocumentFinder(InMemoryDatabase database)
        {
            this.database = database;
        }

        public Task Handle(DocumentRegistered @event)
        {
            database.Table<DocumentListItem>()
                    .Add(new DocumentListItem
                    {
                        Id = @event.Id,
                        Url = @event.Url
                    });

            return Task.Delay(0);
        }

        public DocumentListItem SearchByUrl(string documentUrl)
        {
            return database.Table<DocumentListItem>().FirstOrDefault(x => x.Url == documentUrl);
        }
    }
}