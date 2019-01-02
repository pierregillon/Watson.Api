using System;
using try4real.ddd;

namespace fakenewsisor.server
{
    public class FactAdded : Event
    {
        public readonly Guid Id;
        public readonly Fact Fact;

        public FactAdded(Guid documentId, Fact fact)
        {
            Id = documentId;
            Fact = fact;
        }
    }
}