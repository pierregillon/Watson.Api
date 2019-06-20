using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CQRSlite.Events;
using Watson.Infrastructure;

namespace Watson.Domain.ListFacts
{
    public class FactFinder : IEventHandler<SuspiciousFactDetected>
    {
        private static readonly Regex Regex = new Regex(@"^http[s]?:\/\/[^?^&]*");

        private readonly InMemoryDatabase _database;

        public FactFinder(InMemoryDatabase database)
        {
            this._database = database;
        }

        public Task<IReadOnlyCollection<FactListItem>> GetAll(string url = null, int? skip = null, int? take = null)
        {
            var query = (IEnumerable<FactListItem>)_database.Table<FactListItem>();
            if (string.IsNullOrEmpty(url) == false) {
                var pageBaseUrl = PageUrlWithoutParameters(url);
                query = query.Where(x => string.Equals(x.PageBaseUrl, pageBaseUrl, StringComparison.InvariantCultureIgnoreCase));
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
            _database.Table<FactListItem>()
                    .Add(new FactListItem {
                        WebPageUrl = @event.WebPageUrl,
                        PageBaseUrl = PageUrlWithoutParameters(@event.WebPageUrl),
                        StartNodeXPath = @event.Location.StartNodeXPath,
                        EndNodeXPath = @event.Location.EndNodeXPath,
                        StartOffset = @event.Location.StartOffset,
                        EndOffset = @event.Location.EndOffset,
                        Wording = @event.Wording
                    });

            return Task.Delay(0);
        }

        private static string PageUrlWithoutParameters(string pageUrl)
        {
            var matches = Regex.Matches(pageUrl.ToLower());
            if (matches.Any()) {
                return matches.First().Value;
            }
            return pageUrl;
        }
    }
}