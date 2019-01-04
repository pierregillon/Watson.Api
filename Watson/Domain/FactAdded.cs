using System;
using CQRSlite.Events;

namespace Watson.Domain
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