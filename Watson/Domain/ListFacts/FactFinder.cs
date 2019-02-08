using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRSlite.Events;
using Watson.Domain;
using Watson.Infrastructure;

namespace Watson.Domain.ListFacts
{
    public class FactFinder : IEventHandler<SuspiciousFactDetected>
    {
        private readonly InMemoryDatabase database;

        public FactFinder(InMemoryDatabase database)
        {
            this.database = database;
        }

        public Task<IReadOnlyCollection<FactListItem>> GetAll(string url = null, int? skip = null, int? take = null)
        {
            var query = (IEnumerable<FactListItem>)database.Table<FactListItem>();
            if (string.IsNullOrEmpty(url) == false) {
                query = query.Where(x => string.Equals(x.WebPageUrl, url.ToLower(), StringComparison.InvariantCultureIgnoreCase));
            }
            if (skip.HasValue) {
                query = query.Skip(skip.Value);
            }
            if (take.HasValue) {
                query = query.Take(take.Value);
            }
            return Task.FromResult((IReadOnlyCollection<FactListItem>)query.ToArray());
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