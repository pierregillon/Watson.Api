using System;
using CQRSlite.Domain;

namespace Watson.Domain
{
    public class User : AggregateRoot
    {
        public User(Guid userId)
        {
            ApplyChange(new UserRegistered(userId));
        }
    }
}