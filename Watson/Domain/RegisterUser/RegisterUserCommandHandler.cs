using System.Threading.Tasks;
using CQRSlite.Commands;
using CQRSlite.Domain;
using CQRSlite.Events;
using CQRSlite.Queries;
using Watson.Domain.FindUser;

namespace Watson.Domain.RegisterUser
{
    public class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand>
    {
        private readonly IRepository repository;
        private readonly IQueryProcessor queryProcessor;

        public RegisterUserCommandHandler(IRepository repository, IQueryProcessor queryProcessor)
        {
            this.repository = repository;
            this.queryProcessor = queryProcessor;
        }

        public async Task Handle(RegisterUserCommand command)
        {
            if (await queryProcessor.Query(new FindUserQuery(command.UserId)) != null) {
                throw new UserAlreadyExists(command.UserId);
            }

            var user = new User(command.UserId);

            await repository.Save(user);
        }
    }
}