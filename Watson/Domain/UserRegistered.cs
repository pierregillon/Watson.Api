using System;
using Watson.Domain;

namespace Watson.Domain
{
    public class UserRegistered : DomainEvent
    {
        public UserRegistered(Guid id)
        {
            this.Id = id;
        }
    }
}