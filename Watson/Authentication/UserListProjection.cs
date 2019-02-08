using System.Collections.Generic;
using System.Threading.Tasks;
using CQRSlite.Events;
using Watson.Infrastructure;

namespace Watson.Authentication
{
    public class UserListProjection : IEventHandler<UserRegistered>
    {
        private readonly InMemoryDatabase database;

        public IEnumerable<UserListItem> Users { get { return database.Table<UserListItem>(); } }

        public UserListProjection(InMemoryDatabase database)
        {
            this.database = database;
        }

        public Task Handle(UserRegistered @event)
        {
            this.database.Table<UserListItem>().Add(new UserListItem {
                Id = @event.Id
            });

            return Task.Delay(0);
        }
    }
}