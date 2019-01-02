using System;
using System.Runtime.Serialization;

namespace fakenewsisor.server
{
    public class EmptyXPath : Exception
    {
        public EmptyXPath() : base("Cannot add fact with empty xpath location.")
        {
        }
    }
}