using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRSlite.Events;
using fakenewsisor.server.Infrastructure;

namespace fakenewsisor.server
{
    public class FactFinder : IEventHandler<FactAdded>
    {
        private readonly InMemoryDatabase database;

        public FactFinder(InMemoryDatabase database)
        {
            this.database = database;
        }

        public Task<IReadOnlyCollection<FactListItem>> GetAll(string documentUrl)
        {
            var data = database.Table<FactListItem>().Where(x => x.documentUrl == documentUrl).ToArray();
            return Task.FromResult(data).ContinueWith(x => (IReadOnlyCollection<FactListItem>)x.Result);
        }

        public Task Handle(FactAdded @event)
        {
            var documentUrl = database.Table<DocumentListItem>().Single(x => x.Id == @event.Id).Url;

            database.Table<FactListItem>()
                    .Add(new FactListItem
                    {
                        documentUrl = documentUrl,
                        firstTextNodeXPath = @event.Fact.Location.FirstSelectedHtmlNodeXPath,
                        lastTextNodeXPath = @event.Fact.Location.LastSelectedHtmlNodeXPath,
                        offsetStart = @event.Fact.Location.SelectedTextStartOffset,
                        offsetEnd = @event.Fact.Location.SelectedTextEndOffset,
                        text = @event.Fact.Text
                    });

            return Task.Delay(0);
        }
    }
}