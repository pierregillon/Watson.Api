using System;
using System.Runtime.Serialization;

namespace Watson.Domain.AddFact
{
    [Serializable]
    internal class DocumentNotFound : Exception
    {
        public DocumentNotFound(string message) : base(message)
        {
        }
    }
}