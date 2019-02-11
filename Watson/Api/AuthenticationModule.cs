using CQRSlite.Commands;
using CQRSlite.Queries;
using Nancy;
using Nancy.ModelBinding;
using Watson.Api.Jwt;
using Watson.Domain.FindUser;
using Watson.Domain.RegisterUser;

namespace Watson.Api
{
    public class AuthenticationModule : NancyModule
    {
        public AuthenticationModule(
            ICommandSender commandSender, 
            ITokenEncoder tokenEncoder,
            IQueryProcessor queryProcessor)
        {
            Post("/api/register", async _ => {
                var command = this.Bind<RegisterUserCommand>();
                await commandSender.Send(command);
                return Negotiate
                    .WithStatusCode(HttpStatusCode.OK)
                    .WithModel(new {
                        token = tokenEncoder.Encode(new JwtPayload(command.UserId))
                    });
            });

            Post("/api/login", async _ => {
                var userId = this.Request.Form.UserId;
                if (string.IsNullOrEmpty(userId)) {
                    return Negotiate.WithStatusCode(HttpStatusCode.Unauthorized);
                }
                var user = await queryProcessor.Query(new FindUserQuery(userId));
                if (user == null) {
                    return Negotiate.WithStatusCode(HttpStatusCode.Unauthorized);
                }
                return Negotiate
                    .WithStatusCode(HttpStatusCode.OK)
                    .WithModel(new {
                        token = tokenEncoder.Encode(new JwtPayload(userId))
                    });
            });
        }
    }
}