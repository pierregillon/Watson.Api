using System;
using System.Threading.Tasks;
using fakenewsisor.server.DDD_CQRS;

namespace fakenewsisor.server
{
    public class RegisterDocumentCommandHandler : ICommandHandler<RegisterDocumentCommand, Guid>
    {
        private readonly IRepository<Document> documentRepository;

        public RegisterDocumentCommandHandler(IRepository<Document> documentRepository)
        {
            this.documentRepository = documentRepository;
        }

        public async Task<Guid> Handle(RegisterDocumentCommand command)
        {
            var document = new Document(command.DocumentUrl);
            await this.documentRepository.Save(document);
            return document.Id;
        }
    }
}