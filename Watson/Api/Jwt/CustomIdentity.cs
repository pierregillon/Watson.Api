using System.Security.Principal;

namespace Watson.Api.Jwt
{
    public class CustomIdentity : IIdentity
    {
        public string AuthenticationType => "bearer token";
        public bool IsAuthenticated => true;
        public string Name {get; private set; }

        public CustomIdentity(string name)
        {
            Name = name;
        }
    }
}