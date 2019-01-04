using System;
using System.Runtime.Serialization;

namespace Watson.Domain.RegisterDocument

{
    public class UnreachableWebDocument : Exception
    {
        public UnreachableWebDocument(string url) : base($"The web page '{url}' is unreachable.")
        {
        }
    }
}