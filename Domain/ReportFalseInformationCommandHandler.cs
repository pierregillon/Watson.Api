using System.Threading.Tasks;

namespace fakenewsisor.server
{
    public class ReportFalseInformationCommandHandler : ICommandHandler<ReportFalseInformationCommand>
    {
        private readonly IWebPageRepository repository;

        public ReportFalseInformationCommandHandler(IWebPageRepository repository)
        {
            this.repository = repository;
        }

        public async Task Handle(ReportFalseInformationCommand command)
        {
            var webPage = await repository.Load(command.webPageUrl);
            if (webPage != null)
            {
                var falseInformation = new FalseInformation { };
                webPage.FalseInformations.Add(falseInformation);
                await repository.Save(webPage);
            }
        }
    }
}