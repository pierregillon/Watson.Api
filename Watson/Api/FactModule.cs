using System;
using System.Threading.Tasks;
using CQRSlite.Commands;
using Nancy.Configuration;
using Nancy.ModelBinding;
using Watson.Domain;
using Watson.Domain.SuspectFalseFact;

namespace Watson.Api
{
    public class FactModule : Nancy.NancyModule
    {
        public FactModule(
            FactFinder factFinder,
            ICommandSender dispatcher) : base()
        {
            Get("/api/fact", async _ =>
            {
                var url = GetUrlQueryParameter();
                return await factFinder.GetAll(url);
            });

            Post("/api/fact", async _ =>
            {
                var command = this.Bind<SuspectFalseFactCommand>();
                command.WebPageUrl = GetUrlQueryParameter();
                await dispatcher.Send(command);
                return "fact added";
            });
        }

        private string GetUrlQueryParameter()
        {
            var url = this.Request.Query["url"];
            if (string.IsNullOrEmpty(url))
                throw new Exception("url parameter must be specified");
            return url;
        }
    }
}