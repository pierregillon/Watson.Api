using System;

namespace Watson.Domain.SuspectFalseFact
{
    public class NotEnoughWords : Exception
    {
        public NotEnoughWords(int minimumCount) : base($"A new suspicious fact wording must contain at least {minimumCount} words.")
        {
        }
    }
}