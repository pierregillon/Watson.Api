using System.Threading.Tasks;
using CQRSlite.Commands;

namespace Watson.Authentication
{
    public class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand>
    {
        public Task Handle(RegisterUserCommand message)
        {
            throw new System.NotImplementedException();
        }
    }
}