using System;
using CQRSlite.Domain;

namespace Watson.Domain.SuspectFalseFact
{
    public class Fact2 : AggregateRoot
    {
        public Fact2(){}
        public Fact2(string wording, string webPageUrl, HtmlLocation htmlLocation)
        {
            if (string.IsNullOrEmpty(wording) || string.IsNullOrWhiteSpace(wording)) {
                throw new ArgumentException("wording", nameof(wording));
            }
            if (string.IsNullOrEmpty(htmlLocation.FirstSelectedHtmlNodeXPath) || string.IsNullOrEmpty(htmlLocation.LastSelectedHtmlNodeXPath)) {
                throw new InvalidHtmlLocation("Both node Xmap should be defined.");
            }
            if(htmlLocation.SelectedTextStartOffset < 0 || htmlLocation.SelectedTextEndOffset < 0 || htmlLocation.SelectedTextStartOffset >= htmlLocation.SelectedTextEndOffset) {
                throw new InvalidHtmlLocationOffsets();
            }
            if (wording.Length != htmlLocation.SelectedTextEndOffset - htmlLocation.SelectedTextStartOffset) {
                throw new FactWordingAndOffsetInconsistent();
            }
            
            ApplyChange(new SuspiciousFactDetected(Guid.NewGuid(), wording, webPageUrl, htmlLocation));
        }

        private void Apply(SuspiciousFactDetected @event) 
        {
        }
    }
}