using CQRSlite.Commands;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Responses.Negotiation;
using Watson.Authentication;

namespace Watson.Api
{
    public class AuthenticationModule : NancyModule
    {
        public AuthenticationModule(ICommandSender commandSender)
        {
            Post("/api/register", async _ => {
                var command = this.Bind<RegisterUserCommand>();
                await commandSender.Send(command);
                return Negotiate.WithStatusCode(HttpStatusCode.OK);
            });

            Post("token", async _ => {
                var command = this.Bind<GetTokenCommand>();
                await commandSender.Send(command);
                return Negotiate
                    .WithStatusCode(HttpStatusCode.OK)
                    .WithModel(new {
                        Token = command.Token
                    });
            });
        }
    }
}