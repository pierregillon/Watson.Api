using System;
using System.Runtime.Serialization;

namespace fakenewsisor.server
{
    public class TooLongFact : Exception
    {
        public TooLongFact(int maxCount) : base($"Can not add too long fact. Limit {maxCount} characters.")
        {
        }
    }
}