using System;
using CQRSlite.Domain;

namespace Watson.Domain.SuspectFalseFact
{
    public class Fact2 : AggregateRoot
    {
        public Fact2(){}
        public Fact2(string factContent, string webPageUrl, HtmlLocation htmlLocation)
        {
            if (string.IsNullOrEmpty(htmlLocation.FirstSelectedHtmlNodeXPath) || string.IsNullOrEmpty(htmlLocation.LastSelectedHtmlNodeXPath)) {
                throw new InvalidHtmlLocation("Both node Xmap should be defined.");
            }
            if (factContent.Length != htmlLocation.SelectedTextEndOffset - htmlLocation.SelectedTextStartOffset) {
                throw new FactTextAndOffsetInconsistent();
            }
            ApplyChange(new SuspiciousFactDetected(Guid.NewGuid(), factContent, webPageUrl, htmlLocation));
        }

        private void Apply(SuspiciousFactDetected @event) 
        {
        }
    }
}