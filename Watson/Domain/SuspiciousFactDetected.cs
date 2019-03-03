using System;

namespace Watson.Domain
{
    public class SuspiciousFactDetected : DomainEvent
    {
        public readonly Guid Reporter;
        public readonly string Wording;
        public readonly string WebPageUrl;
        public readonly HtmlLocation Location;

        public SuspiciousFactDetected(Guid id, Guid reporter, string wording, string webPageUrl, HtmlLocation location) : base(id)
        {
            this.Id = id;
            this.Reporter = reporter;
            this.Wording = wording;
            this.WebPageUrl = webPageUrl;
            this.Location = location;
        }
    }
}