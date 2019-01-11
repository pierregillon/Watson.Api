using System.Threading.Tasks;
using CQRSlite.Commands;
using CQRSlite.Domain;

namespace Watson.Domain.ReportSuspiciousFact
{
    public class ReportSuspiciousFactCommandHandler : ICommandHandler<ReportSuspiciousFactCommand>
    {
        private readonly IRepository repository;
        private readonly IWebSiteChecker webSiteChecker;

        public ReportSuspiciousFactCommandHandler(IRepository repository, IWebSiteChecker webSiteChecker)
        {
            this.repository = repository;
            this.webSiteChecker = webSiteChecker;
        }

        public async Task Handle(ReportSuspiciousFactCommand command)
        {
            if (await webSiteChecker.IsOnline(command.WebPageUrl) == false) {
                throw new UnreachableWebPage(command.WebPageUrl);
            }

            var fact = new Fact(command.Wording, command.WebPageUrl, new HtmlLocation {
                StartNodeXPath = XPath.Parse(command.FirstSelectedHtmlNodeXPath),
                EndNodeXPath = XPath.Parse(command.LastSelectedHtmlNodeXPath),
                StartOffset = command.SelectedTextStartOffset,
                EndOffset = command.SelectedTextEndOffset
            });

            await repository.Save(fact);
        }
    }
}