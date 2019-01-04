using System;
using CQRSlite.Commands;

namespace Watson.Domain.RegisterDocument
{
    public class RegisterDocumentCommand : ICommand
    {
        public Guid RegisteredDocumentId { get; internal set; }
        public readonly string DocumentUrl;

        public RegisterDocumentCommand(string documentUrl)
        {
            this.DocumentUrl = documentUrl;
        }

    }
}