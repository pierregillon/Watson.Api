using System.Threading.Tasks;
using Nancy.Configuration;
using Nancy.ModelBinding;

namespace fakenewsisor.server
{
    public class FalseInformationModule : Nancy.NancyModule
    {
        public FalseInformationModule(
            FalseInformationFinder falseInformationFinder,
            ICommandDispatcher dispatcher) : base("/api/falseinformation")
        {
            Get("/{webPageUrl}", async parameters =>
            {
                return await falseInformationFinder.GetAll(parameters.webPageUrl);
            });

            Post("/", async parameters =>
            {
                var command = this.Bind<ReportFalseInformationCommand>();
                await dispatcher.Dispatch(command);
            });
        }
    }
}