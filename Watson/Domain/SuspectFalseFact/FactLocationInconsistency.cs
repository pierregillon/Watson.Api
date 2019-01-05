using System;

namespace Watson.Domain.SuspectFalseFact
{
    public class FactWordingAndOffsetInconsistent : Exception
    {
        public FactWordingAndOffsetInconsistent() : base("Fact inconsistency between offset location and text.")
        {
        }
    }
}