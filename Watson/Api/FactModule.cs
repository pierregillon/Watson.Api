using System;
using System.Text;
using System.Threading.Tasks;
using CQRSlite.Commands;
using Nancy.Configuration;
using Nancy.ModelBinding;
using Watson.Domain;
using Watson.Domain.ReportSuspiciousFact;

namespace Watson.Api
{
    public class FactModule : Nancy.NancyModule
    {
        public FactModule(FactFinder factFinder, ICommandSender dispatcher) : base()
        {
            Get("/api/fact", async _ => {
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
            });

            Post("/api/fact", async _ => {
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
            });
        }
    }
}