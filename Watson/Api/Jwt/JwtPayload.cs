using System;

namespace Watson.Api.Jwt
{
    public class JwtPayload
    {
        public Guid UserId { get; set; }
        public DateTime Expire { get; set; } = DateTime.UtcNow.AddMinutes(30);
    }
}