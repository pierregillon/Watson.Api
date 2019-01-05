using System.Threading.Tasks;
using CQRSlite.Commands;
using CQRSlite.Domain;
using Watson.Domain.RegisterDocument;

namespace Watson.Domain.SuspectFalseFact
{
    public class SuspectFalseFactCommandHandler : ICommandHandler<SuspectFalseFactCommand>
    {
        private readonly IRepository repository;
        private readonly IWebSiteChecker webSiteChecker;

        public SuspectFalseFactCommandHandler(IRepository repository, IWebSiteChecker webSiteChecker)
        {
            this.repository = repository;
            this.webSiteChecker = webSiteChecker;
        }

        public async Task Handle(SuspectFalseFactCommand command)
        {
            if (await webSiteChecker.IsOnline(command.WebPageUrl) == false) {
                throw new UnreachableWebPage(command.WebPageUrl);
            }

            var fact = new Fact2(command.Wording, command.WebPageUrl, new HtmlLocation (
                command.FirstSelectedHtmlNodeXPath,
                command.LastSelectedHtmlNodeXPath,
                command.SelectedTextStartOffset,
                command.SelectedTextEndOffset
            ));

            await repository.Save(fact);
        }
    }
}