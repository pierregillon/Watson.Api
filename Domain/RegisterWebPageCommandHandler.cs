using System;
using System.Threading.Tasks;
using fakenewsisor.server.DDD_CQRS;

namespace fakenewsisor.server
{
    public class RegisterWebPageCommandHandler : ICommandHandler<RegisterWebPageCommand, Guid>
    {
        private readonly IRepository<WebPage> webPageRepository;

        public RegisterWebPageCommandHandler(IRepository<WebPage> webPageRepository)
        {
            this.webPageRepository = webPageRepository;
        }

        public async Task<Guid> Handle(RegisterWebPageCommand command)
        {
            var webPage = new WebPage(command.WebPageUrl);
            await this.webPageRepository.Save(webPage);
            return webPage.Id;
        }
    }
}