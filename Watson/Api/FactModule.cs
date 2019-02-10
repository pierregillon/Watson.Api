using System;
using System.Security.Claims;
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
                var base64Url = (string)this.Request.Query.url;
                var listFacts = new ListFactsQuery (
                    string.IsNullOrEmpty(base64Url) == false ? Encoding.UTF8.GetString(Convert.FromBase64String(base64Url)) : null,
                    (int?)this.Request.Query.skip,
                    (int?)this.Request.Query.take
                );
                return await queryProcessor.Query(listFacts);
            });

            Post("/api/fact", async _ => {
                try {
                    var command = new ReportSuspiciousFactCommand() {
                        StartNodeXPath = Request.Form.startNodeXPath,
                        StartOffset = Request.Form.startOffset,
                        EndNodeXPath = Request.Form.endNodeXPath,
                        EndOffset = Request.Form.endOffset,
                        Reporter = Guid.Parse(Context.CurrentUser.FindFirstValue("userId")),
                        WebPageUrl = GetFactUrl(),
                        Wording = Request.Form.wording
                    };
                    await dispatcher.Send(command);
                    return Negotiate.WithStatusCode(HttpStatusCode.OK);
                }
                catch (DomainException ex) {
                    return BadRequest(ex);
                }
            });
        }

        private Negotiator BadRequest(DomainException ex) {
            return Negotiate
                    .WithStatusCode(HttpStatusCode.BadRequest)
                    .WithModel(new {
                        message = ex.Message
                    });
        }

        private string GetFactUrl() {
            var base64Url = (string)this.Request.Query["url"];
            if (string.IsNullOrEmpty(base64Url)) {
                throw new Exception("fact url parameter must be specified");
            }
            var url = Encoding.UTF8.GetString(Convert.FromBase64String(base64Url)); ;
            if (string.IsNullOrEmpty(url)) {
                throw new Exception("fact url parameter must be specified");
            }
            return url;
        }
    }
}