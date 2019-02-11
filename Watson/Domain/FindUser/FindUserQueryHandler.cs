using System.Linq;
using System.Threading.Tasks;
using CQRSlite.Queries;

namespace Watson.Domain.FindUser
{
    public class FindUserQueryHandler : IQueryHandler<FindUserQuery, UserListItem>
    {
        private readonly UserListProjection projection;

        public FindUserQueryHandler(UserListProjection projection)
        {
            this.projection = projection;
        }

        public Task<UserListItem> Handle(FindUserQuery query)
        {
            return Task.FromResult(this.projection.Users.FirstOrDefault(x => x.Id == query.UserId));
        }
    }
}