using System.Collections.Generic;
using System.Security.Claims;

namespace Watson.Api.Jwt
{
    public interface ITokenEncoder
    {
        string Encode(JwtPayload payload);
        JwtPayload Decode(string token);
    }
}