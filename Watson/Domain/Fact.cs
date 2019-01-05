using System;
using CQRSlite.Domain;

namespace Watson.Domain.SuspectFalseFact
{
    public class Fact : AggregateRoot
    {
        public Fact(){}
        public Fact(string wording, string webPageUrl, HtmlLocation htmlLocation)
        {
            if (string.IsNullOrEmpty(wording) || string.IsNullOrWhiteSpace(wording)) {
                throw new ArgumentException("wording", nameof(wording));
            }
            
            htmlLocation.AssertValid(wording);
            
            ApplyChange(new SuspiciousFactDetected(Guid.NewGuid(), wording, webPageUrl, htmlLocation));
        }

        private void Apply(SuspiciousFactDetected @event) 
        {
        }
    }
}