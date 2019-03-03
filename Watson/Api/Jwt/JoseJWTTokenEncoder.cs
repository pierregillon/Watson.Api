using System.Text;
using Jose;
using Newtonsoft.Json;
using Watson.Server;

namespace Watson.Api.Jwt
{
    public class JoseJwtTokenEncoder : ITokenEncoder
    {
        private readonly AppSettings settings;

        public JoseJwtTokenEncoder(AppSettings settings)
        {
            this.settings = settings;
        }

        public string Encode(JwtPayload payload)
        {
            var secretKey = Encoding.UTF8.GetBytes(settings.Jwt.SecretKey);
            return Jose.JWT.Encode(payload, secretKey, JwsAlgorithm.HS256);
        }

        public JwtPayload Decode(string token)
        {
            var secretKey = Encoding.UTF8.GetBytes(settings.Jwt.SecretKey);
            var jsonPayload = Jose.JWT.Decode(token, secretKey, JwsAlgorithm.HS256);
            return JsonConvert.DeserializeObject<JwtPayload>(jsonPayload);
        }
    }
}