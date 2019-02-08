using System.Threading.Tasks;
using CQRSlite.Commands;

namespace Watson.Authentication
{
    public class GetTokenCommandHandler : ICommandHandler<GetTokenCommand>
    {
        public Task Handle(GetTokenCommand message)
        {
            throw new System.NotImplementedException();
        }
    }
}