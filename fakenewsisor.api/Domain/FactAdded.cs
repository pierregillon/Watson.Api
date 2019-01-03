using System;
using CQRSlite.Events;

namespace fakenewsisor.server
{
    public class FactAdded : DomainEvent
    {
        public readonly Fact Fact;

        public FactAdded(Guid documentId, Fact fact) : base(documentId)
        {
            Id = documentId;
            Fact = fact;
        }
    }
}