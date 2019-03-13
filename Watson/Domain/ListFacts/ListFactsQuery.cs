using System.Collections.Generic;
using CQRSlite.Queries;

namespace Watson.Domain.ListFacts
{
    public class ListFactsQuery : IQuery<IReadOnlyCollection<FactListItem>> {
        public string Url { get; }
        public int? Skip { get; }
        public int? Take { get; }

        public ListFactsQuery(string url = null, int? skip = null, int? take = null)
        {
            Url = url;
            Skip = skip;
            Take = take;
        }
    }
}