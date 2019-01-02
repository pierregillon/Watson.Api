using System;
using System.Runtime.Serialization;

namespace fakenewsisor.server
{
    [Serializable]
    internal class DocumentNotFound : Exception
    {
        public DocumentNotFound(string message) : base(message)
        {
        }
    }
}