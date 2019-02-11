using System;
using System.Security.Claims;
using CQRSlite.Queries;
using System.Linq;
using Watson.Domain.FindUser;

namespace Watson.Api.Jwt
{
    public class JwtTokenValidator : ITokenValidator
    {
        private readonly ITokenEncoder tokenGenerator;
        private readonly IQueryProcessor queryProcessor;

        public JwtTokenValidator(ITokenEncoder tokenGenerator, IQueryProcessor queryProcessor)
        {
            this.tokenGenerator = tokenGenerator;
            this.queryProcessor = queryProcessor;
        }

        public ClaimsPrincipal ValidateUser(string token)
        {
            var jwtPayload = tokenGenerator.Decode(token);

            if (jwtPayload.Expire < DateTime.UtcNow) {
                return null;
            }

            if (queryProcessor.Query(new FindUserQuery(jwtPayload.UserId)).Result == null) {
                return null;
            }

            return new ClaimsPrincipal(new [] {
                new ClaimsIdentity(
                    new CustomIdentity(jwtPayload.UserId.ToString()), 
                    jwtPayload.Claims.Select(x => new Claim(x.Key, x.Value))
                )
            });
        }
    }
}