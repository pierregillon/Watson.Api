using System;

namespace Watson.Domain.SuspectFalseFact
{
    public class InvalidHtmlLocation : Exception
    {
        public InvalidHtmlLocation(string message) : base(message)
        {
        }
    }
}