using System.Threading.Tasks;
using fakenewsisor.server.DDD_CQRS;

namespace fakenewsisor.server
{
    public class AddFactCommandHandler : ICommandHandler<AddFactCommand>
    {
        private readonly IRepository<Document> documentRepository;

        public AddFactCommandHandler(IRepository<Document> documentRepository)
        {
            this.documentRepository = documentRepository;
        }

        public async Task Handle(AddFactCommand command)
        {
            var document = await documentRepository.Load(command.documentId);
            if (document == null)
                throw new DocumentNotFound($"Unable to load {command.documentId}");

            var fact = new Fact
            {
                Location = new HtmlLocation()
                {
                    FirstSelectedHtmlNodeXPath = command.firstTextNodeXPath,
                    LastSelectedHtmlNodeXPath = command.lastTextNodeXPath,
                    SelectedTextStartOffset = command.offsetStart,
                    SelectedTextEndOffset = command.offsetEnd,
                },
                Text = command.text
            };
            document.Add(fact);
            await documentRepository.Save(document);
        }
    }
}