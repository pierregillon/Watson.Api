using System;

namespace Watson.Domain

{
    public class DocumentRegistered : DomainEvent
    {
        public readonly string Url;

        public DocumentRegistered(Guid documentId, string documentUrl) : base(documentId)
        {
            Url = documentUrl;
        }
    }
}