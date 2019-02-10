using System;
using System.Collections.Generic;

namespace Watson.Api.Jwt
{
    public class JwtPayload
    {
        public Guid UserId { get; set; }
        public DateTime Expire { get; } = DateTime.UtcNow.AddMinutes(30);
        public IDictionary<string, string> Claims { get; set; } = new Dictionary<string, string>();

        public JwtPayload(Guid userId)
        {
            UserId = userId;
            Claims.Add("userId", userId.ToString());
        }
    }
}