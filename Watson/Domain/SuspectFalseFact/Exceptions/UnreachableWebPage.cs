using System;
using System.Runtime.Serialization;

namespace Watson.Domain.SuspectFalseFact
{
    public class UnreachableWebPage : Exception
    {
        public UnreachableWebPage(string url) : base($"The web page '{url}' is unreachable.")
        {
        }
    }
}