using System;
using try4real.ddd;

namespace fakenewsisor.server
{
    public class DocumentRegistered : Event
    {
        public readonly Guid DocumentId;
        public readonly string DocumentUrl;

        public DocumentRegistered(Guid documentId, string documentUrl)
        {
            DocumentId = documentId;
            DocumentUrl = documentUrl;
        }
    }
}