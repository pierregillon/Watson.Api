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
            ICommandDispatcher dispatcher) : base("/api/falseinformation")
        {
            Get("/{webPageUrl}", async parameters =>
            {
                return await falseInformationFinder.GetAll(parameters.webPageUrl);
            });

            Post("/{webPageUrl}", async parameters =>
            {
                var webPage = webPageFinder.SearchByUrl(parameters.webPageUrl);
                if (webPage == null)
                {
                    await dispatcher.Dispatch(new RegisterWebPageCommand(parameters.webPageUrl));
                }
                var command = this.Bind<ReportFalseInformationCommand>();
                command.webPageId = webPage.Id;
                await dispatcher.Dispatch(command);
            });
        }
    }
}