using System;
using Watson.Domain;

namespace Watson.Authentication
{
    public class UserAlreadyExists : DomainException
    {
        public UserAlreadyExists(Guid userId) : base($"The user '{userId}' already exists.")
        {
        }
    }
}