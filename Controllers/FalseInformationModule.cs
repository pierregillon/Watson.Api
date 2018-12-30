using System.Threading.Tasks;
using Nancy.ModelBinding;

namespace fakenewsisor.server
{
    public class FalseInformationModule : Nancy.NancyModule
    {
        public FalseInformationModule(
            FalseInformationFinder falseInformationFinder,
            ICommandDispatcher dispatcher) : base("/api/falseinformation")
        {
            Get("/{siteUrl}", async parameters =>
            {
                return await falseInformationFinder.GetAll(parameters.siteUrl);
            });

            Post("/", async parameters =>
            {
                var command = this.Bind<CreateFalseInformationCommand>();
                await dispatcher.Dispatch(command);
            });
        }
    }

    public class CreateFalseInformationCommand
    {
        public int siteId { get; set; }
        public string text { get; internal set; }
        public string firstTextNodeXPath { get; internal set; }
        public string lastTextNodeXPath { get; internal set; }
        public int offsetStart { get; internal set; }
        public int offsetEnd { get; internal set; }
    }

    public interface ICommandDispatcher
    {
        Task Dispatch<T>(T command);
    }
}