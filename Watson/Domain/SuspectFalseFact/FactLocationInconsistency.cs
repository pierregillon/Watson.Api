using System;

namespace Watson.Domain.SuspectFalseFact
{
    public class FactTextAndOffsetInconsistent : Exception
    {
        public FactTextAndOffsetInconsistent() : base("Fact inconsistency between offset location and text.")
        {
        }
    }
}