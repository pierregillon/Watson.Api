namespace fakenewsisor.server
{
    public class RegisterWebPageCommand
    {
        public readonly string WebPageUrl;

        public RegisterWebPageCommand(string webPageUrl)
        {
            this.WebPageUrl = webPageUrl;
        }
    }
}