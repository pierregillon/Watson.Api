using System;

namespace Watson.Domain.ReportSuspiciousFact
{
    public class InvalidXPathFormat : Exception
    {
        public InvalidXPathFormat(string message) : base(message)
        {
        }
    }
}