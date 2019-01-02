using System;
using System.Collections.Generic;
using try4real.utils;

namespace try4real.ddd
{
    public abstract class AggregateRoot
    {
        private readonly List<Event> _changes = new List<Event>();

        public abstract Guid Id { get; protected set; }
        public int Version { get; internal set; }

        public IEnumerable<Event> GetUncommittedChanges()
        {
            return _changes;
        }

        public void MarkChangesAsCommitted()
        {
            _changes.Clear();
        }

        public void LoadsFromHistory(IEnumerable<Event> history)
        {
            foreach (var e in history)
            {
                ApplyChange(e, false);
            }
        }

        protected void ApplyChange(Event @event)
        {
            ApplyChange(@event, true);
        }

        private void ApplyChange(Event @event, bool isNew)
        {
            this.AsDynamic().Apply(@event);
            if (isNew)
            {
                _changes.Add(@event);
            }
        }
    }
}