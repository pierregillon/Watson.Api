using System;
using CQRSlite.Commands;

namespace fakenewsisor.server
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