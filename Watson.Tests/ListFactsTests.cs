using System;
using System.Linq;
using System.Threading.Tasks;
using Watson.Domain;
using Watson.Domain.ListFacts;
using Watson.Infrastructure;
using Xunit;

namespace Watson.Tests
{
    public class ListFactsTests
    {
        private readonly InMemoryDatabase _database;
        private readonly FactFinder _factFinder;
        private string SOME_URL = "https://www.liberation.fr/politiques/2019/06/18/chasse-aux-chomeurs_1734596";

        public ListFactsTests()
        {
            _database = new InMemoryDatabase();
            _factFinder = new FactFinder(_database);
        }

        [Fact]
        public async Task no_facts_reported_return_empty_results()
        {
            var results = await _factFinder.GetAll();

            Assert.Empty(results);
        }

        [Fact]
        public async Task get_all_facts_even_if_page_parameters_may_be_different()
        {
            await _factFinder.Handle(new SuspiciousFactDetected(
                Guid.NewGuid(), 
                Guid.NewGuid(), 
                "La macronie en attend une diminution du nombre de demandeurs d’emploi. ",
                "https://www.liberation.fr/politiques/2019/06/18/chasse-aux-chomeurs_1734596&param=1", 
                new HtmlLocation()));

            await _factFinder.Handle(new SuspiciousFactDetected(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Il est parfois utile de réformer le marché du travail, quitte à mécontenter.",
                "https://www.liberation.fr/politiques/2019/06/18/chasse-aux-chomeurs_1734596&param=2",
                new HtmlLocation()));

            var results = await _factFinder.GetAll("https://www.liberation.fr/politiques/2019/06/18/chasse-aux-chomeurs_1734596&param=3");

            Assert.Equal(2, results.Count);
            Assert.Equal("https://www.liberation.fr/politiques/2019/06/18/chasse-aux-chomeurs_1734596&param=1", results.ElementAt(0).WebPageUrl);
            Assert.Equal("https://www.liberation.fr/politiques/2019/06/18/chasse-aux-chomeurs_1734596&param=2", results.ElementAt(1).WebPageUrl);
        }
    }
}
