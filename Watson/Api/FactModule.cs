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
                var url = GetUrlQueryParameter();
                if (string.IsNullOrEmpty(url)) {
                    return await factFinder.GetAll();
                }
                else {
                    return await factFinder.Get(url);
                }
            });

            Post("/api/fact", async _ => {
                var command = this.Bind<ReportSuspiciousFactCommand>();
                command.WebPageUrl = GetUrlQueryParameter();
                await dispatcher.Send(command);
                return "fact added";
            });
        }

        private string GetUrlQueryParameter()
        {
            var base64Url = (string)this.Request.Query["url"];
            if (base64Url == null) {
                return null;
            }
            var url = Encoding.UTF8.GetString(Convert.FromBase64String(base64Url));;
            if (string.IsNullOrEmpty(url))
                throw new Exception("url parameter must be specified");
            return url;
        }
    }
}