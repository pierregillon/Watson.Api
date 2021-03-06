using System;
using CQRSlite.Events;

namespace Watson.Domain
{
    public abstract class DomainEvent : IEvent
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
        public DateTimeOffset TimeStamp { get; set; }

        public DomainEvent(Guid id)
        {
            Id = id;
        }

        public DomainEvent()
        {
        }
    }
}