using System;
using System.Collections.Generic;

namespace Watson.Api.Jwt
{
    public class JwtPayload
    {
        public Guid UserId { get; set; }
        public DateTime Expire { get; set; }
        public IDictionary<string, string> Claims { get; set; } = new Dictionary<string, string>();

        public JwtPayload(Guid userId) {
            UserId = userId;
            Expire = DateTime.UtcNow.AddSeconds(10);
            Claims.Add("userId", userId.ToString());
        }
    }
}