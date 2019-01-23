using System;

namespace Watson.Domain
{
    public class SuspiciousFactDetected : DomainEvent
    {
        public readonly string Wording;
        public readonly string WebPageUrl;
        public readonly HtmlLocation Location;

        public SuspiciousFactDetected(Guid id, string factContent, string webPageUrl, HtmlLocation location) : base(id)
        {
            this.Id = id;
            this.Wording = factContent;
            this.WebPageUrl = webPageUrl;
            this.Location = location;
        }
    }
}