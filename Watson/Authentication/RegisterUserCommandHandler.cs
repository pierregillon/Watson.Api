using System.Threading.Tasks;
using CQRSlite.Commands;
using CQRSlite.Domain;
using CQRSlite.Events;
using CQRSlite.Queries;

namespace Watson.Authentication
{
    public class RegisterUserCommandHandler : ICommandHandler<LogUserInCommand>
    {
        private readonly IRepository repository;
        private readonly IQueryProcessor queryProcessor;

        public RegisterUserCommandHandler(IRepository repository, IQueryProcessor queryProcessor)
        {
            this.repository = repository;
            this.queryProcessor = queryProcessor;
        }

        public async Task Handle(LogUserInCommand command)
        {
            if (await queryProcessor.Query(new FindUserQuery(command.UserId)) != null) {
                throw new UserAlreadyExists(command.UserId);
            }

            var user = new User(command.UserId);

            await repository.Save(user);
        }
    }
}