using System.Collections.Generic;
using System.Threading.Tasks;
using fakenewsisor.server.DDD_CQRS;

namespace fakenewsisor.server
{
    public class FalseInformationFinder : IEventListener<FalseInformationReported>
    {
        private IList<FalseInformationReadModel> _falseInformations = new List<FalseInformationReadModel>();

        public Task<IReadOnlyCollection<FalseInformationReadModel>> GetAll(string webPageUrl)
        {
            return Task.FromResult(_falseInformations).ContinueWith(x => (IReadOnlyCollection<FalseInformationReadModel>)x.Result);
        }

        public async Task On(FalseInformationReported @event)
        {
            _falseInformations.Add(new FalseInformationReadModel
            {
                firstTextNodeXPath = @event.FalseInformation.FirstSelectedHtmlNodeXPath,
                lastTextNodeXPath = @event.FalseInformation.LastSelectedHtmlNodeXPath,
                offsetStart = @event.FalseInformation.SelectedTextStartOffset,
                offsetEnd = @event.FalseInformation.SelectedTextEndOffset,
                text = @event.FalseInformation.SelectedText
            });

            await Task.Delay(0);
        }
    }
}