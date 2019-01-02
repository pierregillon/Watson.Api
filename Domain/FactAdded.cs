using System;
using fakenewsisor.server.DDD_CQRS;

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