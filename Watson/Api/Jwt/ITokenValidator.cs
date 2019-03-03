using System.Security.Claims;

namespace Watson.Api.Jwt
{
    public interface ITokenValidator
    {
        ClaimsPrincipal ValidateUser(string token);
    }
}