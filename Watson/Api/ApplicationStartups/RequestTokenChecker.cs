using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Nancy;
using Nancy.Bootstrapper;
using Watson.Api.Jwt;
using Watson.Infrastructure.Logging;

namespace Watson.Api
{
    public class RequestTokenChecker : IRequestStartup {

        private static readonly string[] _publicRoutes = { "/api/login", "/api/register", "api/ping" };
        private readonly ITokenValidator tokenValidator;
        private readonly ILogger logger;

        public RequestTokenChecker(ITokenValidator tokenValidator, ILogger logger)
        {
            this.tokenValidator = tokenValidator;
            this.logger = logger;
        }

        public void Initialize(IPipelines pipelines, NancyContext context)
        {
            pipelines.BeforeRequest.AddItemToStartOfPipeline(CheckToken);
        }

        private async Task<Response> CheckToken(NancyContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(0);

            try {
                var route = Uri.UnescapeDataString(context.Request.Path);
                if(_publicRoutes.Contains(route)) {
                    return null;
                }
                var user = GetUserFromRequest(context.Request);
                if (user == null) {
                    return Unauthorized();
                }
                context.CurrentUser = user;
                return null;
            }
            catch (System.Exception ex) {
                var wrapper = new Exception("An error occured during token checking for the request.", ex);
                await logger.Log(new ErrorLogEntry(wrapper, context));
                return Unauthorized();
            }
        }

        private ClaimsPrincipal GetUserFromRequest(Request request) {
            string token = request.Headers.Authorization;
            if (string.IsNullOrWhiteSpace(token)) {
                return null;
            }
            return tokenValidator.ValidateUser(token);
        }

        private Response Unauthorized()
        {
            return new Response()
                .WithStatusCode(HttpStatusCode.Unauthorized)
                .WithHeader("WWW-Authenticate", "/api/login");
        }
    }
}