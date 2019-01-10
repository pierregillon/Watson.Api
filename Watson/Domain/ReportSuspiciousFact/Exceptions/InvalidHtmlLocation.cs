using System;

namespace Watson.Domain.ReportSuspiciousFact
{
    public class InvalidHtmlLocation : Exception
    {
        public InvalidHtmlLocation(string message) : base(message)
        {
        }
    }
}