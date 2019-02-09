using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Nancy;
using Nancy.Bootstrapper;
using Watson.Api.Jwt;

namespace Watson.Api
{
    public class RequestTokenChecker : IRequestStartup {

        private static readonly string[] _publicRoutes = { "/api/login", "/api/register", "api/ping" };
        private readonly ITokenValidator tokenValidator;

        public RequestTokenChecker(ITokenValidator tokenValidator)
        {
            this.tokenValidator = tokenValidator;
        }

        public void Initialize(IPipelines pipelines, NancyContext context)
        {
            pipelines.BeforeRequest.AddItemToStartOfPipeline(CheckToken);
        }

        private async Task<Response> CheckToken(NancyContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(0);

            var route = Uri.UnescapeDataString(context.Request.Path);
            if(_publicRoutes.Contains(route)) {
                return null;
            }

            string token = context.Request.Headers.Authorization;
            if (string.IsNullOrWhiteSpace(token)) {
                return AuthChallengeResponse(context);
            }

            var user = tokenValidator.ValidateUser(token);
            if (user == null) {
                return AuthChallengeResponse(context);
            }

            context.CurrentUser = user;

            return null;
        }

        private Response AuthChallengeResponse(NancyContext context)
        {
            return new Response()
                .WithStatusCode(HttpStatusCode.Unauthorized)
                .WithHeader("WWW-Authenticate", "/api/login");
        }
    }
}