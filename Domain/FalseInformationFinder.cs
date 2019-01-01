using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fakenewsisor.server.DDD_CQRS;

namespace fakenewsisor.server
{
    public class FalseInformationFinder : IEventListener<FalseInformationReported>
    {
        private readonly InMemoryDatabase database;

        public FalseInformationFinder(InMemoryDatabase database)
        {
            this.database = database;
        }

        public Task<IReadOnlyCollection<FalseInformationReadModel>> GetAll(string webPageUrl)
        {
            var data = database.Table<FalseInformationReadModel>().Where(x => x.webPageUrl == webPageUrl).ToArray();
            return Task.FromResult(data).ContinueWith(x => (IReadOnlyCollection<FalseInformationReadModel>)x.Result);
        }

        public void On(FalseInformationReported @event)
        {
            var webPageUrl = database.Table<WebPageListItem>().Single(x => x.Id == @event.Id).Url;

            database.Table<FalseInformationReadModel>()
                    .Add(new FalseInformationReadModel
                    {
                        webPageUrl = webPageUrl,
                        firstTextNodeXPath = @event.FalseInformation.FirstSelectedHtmlNodeXPath,
                        lastTextNodeXPath = @event.FalseInformation.LastSelectedHtmlNodeXPath,
                        offsetStart = @event.FalseInformation.SelectedTextStartOffset,
                        offsetEnd = @event.FalseInformation.SelectedTextEndOffset,
                        text = @event.FalseInformation.SelectedText
                    });
        }
    }
}