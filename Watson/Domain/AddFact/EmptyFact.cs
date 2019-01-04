using System;

namespace Watson.Domain.AddFact
{
    public class EmptyFact : Exception
    {
        public EmptyFact() : base("Can not add empty fact")
        {
        }
    }
}