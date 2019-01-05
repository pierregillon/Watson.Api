using System;
using CQRSlite.Domain;

namespace Watson.Domain.SuspectFalseFact
{
    public class Fact2 : AggregateRoot
    {
        public Fact2(){}
        public Fact2(string factContent, string webPageUrl, HtmlLocation htmlLocation)
        {
            ApplyChange(new SuspiciousFactDetected(Guid.NewGuid(), factContent, webPageUrl, htmlLocation));
        }

        private void Apply(SuspiciousFactDetected @event) 
        {
        }
    }
}