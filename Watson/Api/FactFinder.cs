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
            var data = database.Table<FactListItem>().Where(x => x.WebPageUrl == webPageUrl).ToArray();
            return Task.FromResult(data).ContinueWith(x => (IReadOnlyCollection<FactListItem>)x.Result);
        }

        public Task Handle(SuspiciousFactDetected @event)
        {
            database.Table<FactListItem>()
                    .Add(new FactListItem {
                        WebPageUrl = @event.WebPageUrl,
                        StartNodeXPath = @event.Location.StartNodeXPath.ToString(),
                        EndNodeXPath = @event.Location.EndNodeXPath.ToString(),
                        StartOffset = @event.Location.StartOffset,
                        EndOffset = @event.Location.EndOffset,
                        Wording = @event.Wording
                    });

            return Task.Delay(0);
        }
    }
}