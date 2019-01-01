using System;
using fakenewsisor.server.DDD_CQRS;

namespace fakenewsisor.server
{
    public class WebPageRegistered : Event
    {
        public readonly Guid Id;
        public readonly string Url;

        public WebPageRegistered(Guid id, string url)
        {
            Id = id;
            Url = url;
        }
    }
}