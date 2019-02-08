using System;
using System.Text;
using System.Threading.Tasks;
using CQRSlite.Commands;
using CQRSlite.Queries;
using Nancy;
using Nancy.Configuration;
using Nancy.ModelBinding;
using Nancy.Responses.Negotiation;
using Newtonsoft.Json;
using Watson.Domain;
using Watson.Domain.ListFacts;
using Watson.Domain.ReportSuspiciousFact;

namespace Watson.Api
{
    public class FactModule : Nancy.NancyModule
    {
        public FactModule(IQueryProcessor queryProcessor, ICommandSender dispatcher) : base()
        {
            Get("/api/fact", async _ => {
                try
                {
                    var base64Url = (string)this.Request.Query["url"];

                    var listFacts = new ListFactsQuery (
                        string.IsNullOrEmpty(base64Url) == false ? Encoding.UTF8.GetString(Convert.FromBase64String(base64Url)) : null,
                        (int?)this.Request.Query["skip"],
                        (int?)this.Request.Query["take"]
                    );

                    return await queryProcessor.Query(listFacts);
                }
                catch (DomainException ex)
                {
                    return BadRequest(ex);
                }
            });

            Post("/api/fact", async _ => {
                try
                {
                    var base64Url = (string)this.Request.Query["url"];
                    if (base64Url == null) {
                        throw new Exception("url parameter must be specified");
                    }
                    var url = Encoding.UTF8.GetString(Convert.FromBase64String(base64Url));;
                    if (string.IsNullOrEmpty(url)) {
                        throw new Exception("url parameter must be specified");
                    }
                    
                    var command = this.Bind<ReportSuspiciousFactCommand>();
                    command.WebPageUrl = url;
                    await dispatcher.Send(command);
                    return "fact added";
                }
                catch (DomainException ex)
                {
                    return BadRequest(ex);
                }
            });
        }

        public Negotiator BadRequest(DomainException ex) {

            return Negotiate
                    .WithStatusCode(HttpStatusCode.BadRequest)
                    .WithModel(new {
                        message = ex.Message
                    });
        }
    }
}