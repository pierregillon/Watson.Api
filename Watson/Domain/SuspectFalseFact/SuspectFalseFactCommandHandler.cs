using System.Threading.Tasks;
using CQRSlite.Commands;
using CQRSlite.Domain;

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

            var fact = new Fact(command.Wording, command.WebPageUrl, new HtmlLocation (
                command.FirstSelectedHtmlNodeXPath,
                command.LastSelectedHtmlNodeXPath,
                command.SelectedTextStartOffset,
                command.SelectedTextEndOffset
            ));

            await repository.Save(fact);
        }
    }
}