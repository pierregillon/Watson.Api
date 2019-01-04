using System.Runtime.Serialization;
using System.Threading.Tasks;
using CQRSlite.Commands;
using CQRSlite.Domain;

namespace Watson.Domain.AddFact
{
    public class AddFactCommandHandler : ICommandHandler<AddFactCommand>
    {
        private readonly IRepository repository;

        public AddFactCommandHandler(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task Handle(AddFactCommand command)
        {
            var document = await repository.Get<Document>(command.documentId);
            if (document == null)
            {
                throw new DocumentNotFound($"Unable to load {command.documentId}");
            }

            CheckCommand(command);

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
            await repository.Save(document);
        }

        private static void CheckCommand(AddFactCommand command)
        {
            if (string.IsNullOrEmpty(command.text))
            {
                throw new EmptyFact();
            }

            if (command.text.Length > 30)
            {
                throw new TooLongFact(30);
            }

            if (string.IsNullOrEmpty(command.firstTextNodeXPath) || string.IsNullOrEmpty(command.lastTextNodeXPath))
            {
                throw new EmptyXPath();
            }
        }
    }
}