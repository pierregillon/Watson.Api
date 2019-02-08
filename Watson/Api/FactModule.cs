using System;
using System.Text;
using System.Threading.Tasks;
using CQRSlite.Commands;
using Nancy;
using Nancy.Configuration;
using Nancy.ModelBinding;
using Newtonsoft.Json;
using Watson.Domain;
using Watson.Domain.ReportSuspiciousFact;

namespace Watson.Api
{
    public class FactModule : Nancy.NancyModule
    {
        public FactModule(FactFinder factFinder, ICommandSender dispatcher) : base()
        {
            Get("/api/fact", async _ => {
                try
                {
                    var base64Url = (string)this.Request.Query["url"];
                    var take = (int?)this.Request.Query["take"];
                    var skip = (int?)this.Request.Query["skip"];
                    if (string.IsNullOrEmpty(base64Url)) {
                        return await factFinder.GetAll(skip, take);
                    }
                    else {
                        var url = Encoding.UTF8.GetString(Convert.FromBase64String(base64Url));;
                        if (string.IsNullOrEmpty(url))
                            throw new Exception("url parameter must be specified");
                        return await factFinder.Get(url, skip, take);
                    }
                }
                catch (DomainException ex)
                {
                    return InvalidRequest(ex);
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
                    return InvalidRequest(ex);
                }
            });
        }

        public Response InvalidRequest(DomainException ex) {
            return new Response() {
                StatusCode = HttpStatusCode.BadRequest,
                ContentType = "text/json",
                Contents = stream => {
                    var json = JsonConvert.SerializeObject(new {
                        message = ex.Message
                    });
                    var bytes = Encoding.UTF8.GetBytes(json);
                    stream.Write(bytes, 0, bytes.Length);
                }
            };
        }
    }
}