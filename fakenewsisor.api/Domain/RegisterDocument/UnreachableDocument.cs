using System;
using System.Runtime.Serialization;

namespace fakenewsisor.server
{
    public class UnreachableDocument : Exception
    {
        public UnreachableDocument(string url) : base($"The web page '{url}' is unreachable.")
        {
        }
    }
}