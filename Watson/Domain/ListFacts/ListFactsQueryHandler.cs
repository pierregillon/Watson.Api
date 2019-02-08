using System.Collections.Generic;
using System.Threading.Tasks;
using CQRSlite.Queries;

namespace Watson.Domain.ListFacts
{
    public class ListFactsQueryHandler : IQueryHandler<ListFactsQuery, IReadOnlyCollection<FactListItem>>
    {
        private readonly FactFinder factFinder;

        public ListFactsQueryHandler(FactFinder factFinder)
        {
            this.factFinder = factFinder;
        }

        public async Task<IReadOnlyCollection<FactListItem>> Handle(ListFactsQuery query)
        {
            return await factFinder.GetAll(query.Url, query.Skip, query.Take);
        }
    }
}