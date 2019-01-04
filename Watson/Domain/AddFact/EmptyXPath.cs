using System;
using System.Runtime.Serialization;

namespace Watson.Domain.AddFact
{
    public class EmptyXPath : Exception
    {
        public EmptyXPath() : base("Cannot add fact with empty xpath location.")
        {
        }
    }
}