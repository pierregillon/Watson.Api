using System;

namespace fakenewsisor.server
{
    public class RegisterWebPageCommand
    {
        public Guid RegisteredWebPageId { get; internal set; }
        public readonly string WebPageUrl;

        public RegisterWebPageCommand(string webPageUrl)
        {
            this.WebPageUrl = webPageUrl;
        }

    }
}