using CQRSlite.Commands;
using Watson.Domain;

namespace Watson.Domain.SuspectFalseFact
{
    public class SuspectFalseFactCommand : ICommand
    {
        public string Wording { get; set; }
        public string WebPageUrl { get; set; }
        public string FirstSelectedHtmlNodeXPath { get; set; }
        public string LastSelectedHtmlNodeXPath { get; set; }
        public int SelectedTextStartOffset { get; set; }
        public int SelectedTextEndOffset { get; set; }
    }
}