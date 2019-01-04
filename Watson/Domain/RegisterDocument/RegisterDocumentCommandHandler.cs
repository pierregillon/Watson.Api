using System;
using System.Threading.Tasks;
using CQRSlite.Commands;
using CQRSlite.Domain;

namespace Watson.Domain.RegisterDocument
{
    public class RegisterDocumentCommandHandler : ICommandHandler<RegisterDocumentCommand>
    {
        private readonly IRepository repository;
        private readonly IWebSiteChecker webSiteChecker;

        public RegisterDocumentCommandHandler(IRepository documentRepository, IWebSiteChecker webSiteChecker)
        {
            this.webSiteChecker = webSiteChecker;
            this.repository = documentRepository;
        }

        public async Task Handle(RegisterDocumentCommand command)
        {
            if (await webSiteChecker.IsOnline(command.DocumentUrl) == false)
                throw new UnreachableWebDocument(command.DocumentUrl);

            var document = new Document(command.DocumentUrl);
            await repository.Save(document);
            command.RegisteredDocumentId = document.Id;
        }
    }
}