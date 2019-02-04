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

        public Task<IReadOnlyCollection<FactListItem>> Get(string webPageUrl, int? skip = null, int? take = null)
        {
            var data = (IEnumerable<FactListItem>)database.Table<FactListItem>().Where(x => x.WebPageUrl == webPageUrl);
            if (skip.HasValue) {
                data = data.Skip(skip.Value);
            }
            if (take.HasValue) {
                data = data.Take(take.Value);
            }
            return Task.FromResult((IReadOnlyCollection<FactListItem>)data.ToArray());
        }

        public Task<IReadOnlyCollection<FactListItem>> GetAll(int? skip = null, int? take = null)
        {
            var data = (IEnumerable<FactListItem>)database.Table<FactListItem>();
            if (skip.HasValue) {
                data = data.Skip(skip.Value);
            }
            if (take.HasValue) {
                data = data.Take(take.Value);
            }
            return Task.FromResult((IReadOnlyCollection<FactListItem>)data.ToArray());
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