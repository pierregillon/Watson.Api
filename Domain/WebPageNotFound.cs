using System;
using System.Runtime.Serialization;

namespace fakenewsisor.server
{
    [Serializable]
    internal class WebPageNotFound : Exception
    {
        public WebPageNotFound()
        {
        }

        public WebPageNotFound(string message) : base(message)
        {
        }

        public WebPageNotFound(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected WebPageNotFound(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}