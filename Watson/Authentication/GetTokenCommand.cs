using CQRSlite.Commands;

namespace Watson.Authentication
{
    public class GetTokenCommand : ICommand
    {
        public dynamic Token { get; internal set; }
    }
}