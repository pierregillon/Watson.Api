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
                if (webPageUrl != null)
                {
                    return await falseInformationFinder.GetAll(webPageUrl);
                }
                return null;
            });

            Post("/api/falseinformation", async parameters =>
            {
                var webPageUrl = this.Request.Query["webPageUrl"];
                if (string.IsNullOrEmpty(webPageUrl))
                {
                    throw new Exception("webPageUrl must be specified");
                }
                Guid webPageId;
                var webPage = webPageFinder.SearchByUrl(webPageUrl);
                if (webPage == null)
                {
                    webPageId = await dispatcher.Dispatch<RegisterWebPageCommand, Guid>(new RegisterWebPageCommand(webPageUrl));
                }
                else
                {
                    webPageId = webPage.Id;
                }
                var command = this.Bind<ReportFalseInformationCommand>();
                command.webPageId = webPageId;
                await dispatcher.Dispatch(command);
                return "ok";
            });
        }
    }
}