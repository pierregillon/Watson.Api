using System;
using System.Runtime.Serialization;

namespace Watson.Domain.ReportSuspiciousFact
{
    public class UnreachableWebPage : Exception
    {
        public UnreachableWebPage(string url) : base($"The web page '{url}' is unreachable.")
        {
        }
    }
}