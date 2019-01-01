using System.Threading.Tasks;
using fakenewsisor.server.DDD_CQRS;

namespace fakenewsisor.server
{
    public class ReportFalseInformationCommandHandler : ICommandHandler<ReportFalseInformationCommand>
    {
        private readonly IRepository<WebPage> repository;

        public ReportFalseInformationCommandHandler(IRepository<WebPage> repository)
        {
            this.repository = repository;
        }

        public async Task Handle(ReportFalseInformationCommand command)
        {
            var webPage = await repository.Load(command.webPageId);
            if (webPage != null)
            {
                var falseInformation = new FalseInformation { };
                webPage.Report(falseInformation);
                await repository.Save(webPage);
            }
        }
    }
}