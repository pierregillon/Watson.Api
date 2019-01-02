using System;
using System.Threading.Tasks;
using fakenewsisor.server.DDD_CQRS;
using Nancy.Configuration;
using Nancy.ModelBinding;

namespace fakenewsisor.server
{
    public class FactModule : Nancy.NancyModule
    {
        public FactModule(
            DocumentFinder documentFinder,
            FactFinder factFinder,
            ICommandDispatcher dispatcher) : base()
        {
            Get("/api/fact", async _ =>
            {
                var url = GetUrlQueryParameter();
                return await factFinder.GetAll(url);
            });

            Post("/api/fact", async _ =>
            {
                var url = GetUrlQueryParameter();
                var command = this.Bind<AddFactCommand>();
                command.documentId = await GetDocumentId(documentFinder, dispatcher, url);
                await dispatcher.Dispatch(command);
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

        private static async Task<Guid> GetDocumentId(DocumentFinder documentFinder, ICommandDispatcher dispatcher, string documentUrl)
        {
            var document = documentFinder.SearchByUrl(documentUrl);
            if (document == null)
                return await dispatcher.Dispatch<RegisterDocumentCommand, Guid>(new RegisterDocumentCommand(documentUrl));
            else
                return document.Id;
        }
    }
}