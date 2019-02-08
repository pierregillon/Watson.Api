using System;
using CQRSlite.Domain;

namespace Watson.Authentication
{
    public class User : AggregateRoot
    {
        public User(Guid userId)
        {
            ApplyChange(new UserRegistered(userId));
        }
    }
}