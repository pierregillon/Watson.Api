using System;

namespace Watson.Domain.SuspectFalseFact
{
    public class InvalidHtmlLocationOffsets : Exception
    {
        public InvalidHtmlLocationOffsets() : base("Html location offsets must be positive and start offset < end offset.")
        {

        }
    }
}