using System;
using System.Runtime.Serialization;

namespace Watson.Domain.AddFact
{
    public class TooLongFact : Exception
    {
        public TooLongFact(int maxCount) : base($"Can not add too long fact. Limit {maxCount} characters.")
        {
        }
    }
}