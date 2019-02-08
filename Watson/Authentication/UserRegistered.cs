using System;
using Watson.Domain;

namespace Watson.Authentication
{
    public class UserRegistered : DomainEvent
    {
        public UserRegistered(Guid id)
        {
            this.Id = id;
        }
    }
}