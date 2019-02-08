using Nancy;

namespace Watson.Api
{
    public class PingModule : Nancy.NancyModule 
    {
        public PingModule() : base()
        {
            Get("api/ping", _ => {
                return Negotiate
                    .WithStatusCode(HttpStatusCode.OK)
                    .WithReasonPhrase("ping ok");
            });
        }
    }
}