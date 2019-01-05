using System.Threading.Tasks;
using CQRSlite.Commands;
using CQRSlite.Domain;

namespace Watson.Domain.SuspectFalseFact
{
    public class SuspectFalseFactCommandHandler : ICommandHandler<SuspectFalseFactCommand>
    {
        private readonly IRepository repository;

        public SuspectFalseFactCommandHandler(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task Handle(SuspectFalseFactCommand command)
        {
            var fact = new Fact2(command.Text, command.WebPageUrl, new HtmlLocation() {
                FirstSelectedHtmlNodeXPath = command.FirstSelectedHtmlNodeXPath,
                LastSelectedHtmlNodeXPath = command.LastSelectedHtmlNodeXPath,
                SelectedTextStartOffset = command.SelectedTextStartOffset,
                SelectedTextEndOffset = command.SelectedTextEndOffset
            });

            await repository.Save(fact);
        }
    }
}