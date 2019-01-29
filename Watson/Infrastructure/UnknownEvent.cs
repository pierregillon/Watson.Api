using System;

namespace Watson.Infrastructure
{
    internal class UnknownEvent : Exception
    {
        public UnknownEvent(string eventName) : base($"The event {eventName} is unknown.")
        {
        }
    }
}