using System;
using CQRSlite.Queries;

namespace Watson.Domain.FindUser
{
    public class FindUserQuery : IQuery<UserListItem>
    {
        public readonly Guid UserId;

        public FindUserQuery(Guid userId)
        {
            this.UserId = userId;
        }
    }
}