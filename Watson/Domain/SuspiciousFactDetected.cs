using System;

namespace Watson.Domain
{
    public class SuspiciousFactDetected : DomainEvent
    {
        public readonly string FactContent;
        public readonly string WebPageUrl;
        public readonly HtmlLocation Location;

        public SuspiciousFactDetected(Guid id, string factContent, string webPageUrl, HtmlLocation htmlLocation) : base(id)
        {
            this.Id = id;
            this.FactContent = factContent;
            this.WebPageUrl = webPageUrl;
            this.Location = htmlLocation;
        }
    }
}