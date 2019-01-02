using System;

namespace fakenewsisor.server
{
    public class RegisterDocumentCommand
    {
        public Guid RegisteredDocumentId { get; internal set; }
        public readonly string DocumentUrl;

        public RegisterDocumentCommand(string documentUrl)
        {
            this.DocumentUrl = documentUrl;
        }

    }
}