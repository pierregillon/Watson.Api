using System;
using System.Threading.Tasks;
using try4real.cqrs;
using try4real.ddd;

namespace fakenewsisor.server
{
    public class RegisterDocumentCommandHandler : ICommandHandler<RegisterDocumentCommand, Guid>
    {
        private readonly IRepository<Document> documentRepository;
        private readonly IWebSiteChecker webSiteChecker;

        public RegisterDocumentCommandHandler(IRepository<Document> documentRepository, IWebSiteChecker webSiteChecker)
        {
            this.webSiteChecker = webSiteChecker;
            this.documentRepository = documentRepository;
        }

        public async Task<Guid> Handle(RegisterDocumentCommand command)
        {
            if (await webSiteChecker.IsOnline(command.DocumentUrl) == false)
                throw new UnreachableWebDocument(command.DocumentUrl);

            var document = new Document(command.DocumentUrl);
            await this.documentRepository.Save(document);
            return document.Id;
        }
    }
}