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
            
            htmlLocation.AssertValid(wording);
            
            ApplyChange(new SuspiciousFactDetected(Guid.NewGuid(), wording, webPageUrl, htmlLocation));
        }

        private void Apply(SuspiciousFactDetected @event) 
        {
        }
    }
}