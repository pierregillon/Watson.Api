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
            var website = await repository.Load(command.webPageId);
        }
    }
}