using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRSlite.Events;
using Watson.Domain;
using Watson.Infrastructure;

namespace Watson.Api
{
    public class FactFinder : IEventHandler<SuspiciousFactDetected>
    {
        private readonly InMemoryDatabase database;

        public FactFinder(InMemoryDatabase database)
        {
            this.database = database;
        }

        public Task<IReadOnlyCollection<FactListItem>> GetAll(string webPageUrl)
        {
            var data = database.Table<FactListItem>().Where(x => x.webPageUrl == webPageUrl).ToArray();
            return Task.FromResult(data).ContinueWith(x => (IReadOnlyCollection<FactListItem>)x.Result);
        }

        public Task Handle(SuspiciousFactDetected @event)
        {
            database.Table<FactListItem>()
                    .Add(new FactListItem {
                        webPageUrl = @event.WebPageUrl,
                        firstTextNodeXPath = @event.Location.StartNodeXPath.ToString(),
                        lastTextNodeXPath = @event.Location.EndNodeXPath.ToString(),
                        startOffset = @event.Location.StartOffset,
                        endOffset = @event.Location.EndOffset,
                        wording = @event.Wording
                    });

            return Task.Delay(0);
        }
    }
}