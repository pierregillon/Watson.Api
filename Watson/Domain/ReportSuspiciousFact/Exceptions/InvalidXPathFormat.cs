using System;

namespace Watson.Domain.ReportSuspiciousFact
{
    public class InvalidXPathFormat : Exception
    {
        public InvalidXPathFormat() : base("Invalid xPath format.")
        {
        }
    }
}