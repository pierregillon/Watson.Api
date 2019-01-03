using System;
using System.Threading.Tasks;
using CQRSlite.Commands;
using Nancy.Configuration;
using Nancy.ModelBinding;

namespace fakenewsisor.server
{
    public class FactModule : Nancy.NancyModule
    {
        public FactModule(
            DocumentFinder documentFinder,
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
                var url = GetUrlQueryParameter();
                var command = this.Bind<AddFactCommand>();
                command.documentId = await GetDocumentId(documentFinder, dispatcher, url);
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

        private static async Task<Guid> GetDocumentId(DocumentFinder documentFinder, ICommandSender commandSender, string documentUrl)
        {
            var document = documentFinder.SearchByUrl(documentUrl);
            if (document == null)
            {
                var command = new RegisterDocumentCommand(documentUrl);
                await commandSender.Send<RegisterDocumentCommand>(command);
                return command.RegisteredDocumentId;
            }
            else
                return document.Id;
        }
    }
}