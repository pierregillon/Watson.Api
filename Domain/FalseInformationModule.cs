using System;
using System.Threading.Tasks;
using fakenewsisor.server.DDD_CQRS;
using Nancy.Configuration;
using Nancy.ModelBinding;

namespace fakenewsisor.server
{
    public class FalseInformationModule : Nancy.NancyModule
    {
        public FalseInformationModule(
            WebPageFinder webPageFinder,
            FalseInformationFinder falseInformationFinder,
            ICommandDispatcher dispatcher) : base()
        {
            Get("/api/falseinformation", async parameters =>
            {
                var webPageUrl = this.Request.Query["webPageUrl"];
                if (string.IsNullOrEmpty(webPageUrl))
                    throw new Exception("webPageUrl must be specified");

                return await falseInformationFinder.GetAll(webPageUrl);
            });

            base.Post("/api/falseinformation", async parameters =>
            {
                var webPageUrl = this.Request.Query["webPageUrl"];
                if (string.IsNullOrEmpty(webPageUrl))
                    throw new Exception("webPageUrl must be specified");

                var command = this.Bind<ReportFalseInformationCommand>();
                command.webPageId = await GetWebPageId(webPageFinder, dispatcher, webPageUrl);
                await dispatcher.Dispatch(command);
                return "created";
            });
        }

        private static async Task<Guid> GetWebPageId(WebPageFinder webPageFinder, ICommandDispatcher dispatcher, string webPageUrl)
        {
            var webPage = webPageFinder.SearchByUrl(webPageUrl);
            if (webPage == null)
                return await dispatcher.Dispatch<RegisterWebPageCommand, Guid>(new RegisterWebPageCommand(webPageUrl));
            else
                return webPage.Id;
        }
    }
}