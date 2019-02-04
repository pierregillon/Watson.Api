using System;

namespace Watson.Domain
{
    public class SuspiciousFactDetected : DomainEvent
    {
        public readonly string Wording;
        public readonly string WebPageUrl;
        public readonly HtmlLocation Location;

        public SuspiciousFactDetected(Guid id, string wording, string webPageUrl, HtmlLocation location) : base(id)
        {
            this.Id = id;
            this.Wording = wording;
            this.WebPageUrl = webPageUrl;
            this.Location = location;
        }
    }
}