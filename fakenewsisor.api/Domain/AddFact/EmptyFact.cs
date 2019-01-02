using System;

namespace fakenewsisor.server
{
    public class EmptyFact : Exception
    {
        public EmptyFact() : base("Can not add empty fact")
        {
        }
    }
}