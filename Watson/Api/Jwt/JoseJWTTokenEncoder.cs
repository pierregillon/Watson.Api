using Jose;
using Newtonsoft.Json;

namespace Watson.Api.Jwt
{
    public class JoseJwtTokenEncoder : ITokenEncoder
    {
        private static byte[] secretKey = new byte[] { 164, 60, 194, 0, 161, 189, 41, 38, 130, 89, 141, 164, 45, 170, 159, 209, 69, 137, 243, 216, 191, 131, 47, 250, 32, 107, 231, 117, 37, 158, 225, 234 };

        public string Encode(JwtPayload payload)
        {
            return Jose.JWT.Encode(payload, secretKey, JwsAlgorithm.HS256);
        }

        public JwtPayload Decode(string token)
        {
            var jsonPayload = Jose.JWT.Decode(token, secretKey, JwsAlgorithm.HS256);
            return JsonConvert.DeserializeObject<JwtPayload>(jsonPayload);
        }
    }
}