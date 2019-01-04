using System;
using System.Collections.Generic;
using CQRSlite.Domain;

namespace Watson.Domain
{
    public class Document : AggregateRoot
    {
        private List<Fact> _facts = new List<Fact>();

        public Document()
        {
        }
        public Document(string url) : this()
        {
            ApplyChange(new DocumentRegistered(Guid.NewGuid(), url));
        }

        public void Add(Fact fact)
        {
            ApplyChange(new FactAdded(Id, fact));
        }

        private void Apply(FactAdded @event)
        {
            _facts.Add(@event.Fact);
        }

        private void Apply(DocumentRegistered @event)
        {
            Id = @event.Id;
        }
    }
}