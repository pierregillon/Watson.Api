using System;

namespace Watson.Domain
{
    public abstract class DomainException : Exception
    {
        public DomainException(string message) : base(message) { }
    }
}