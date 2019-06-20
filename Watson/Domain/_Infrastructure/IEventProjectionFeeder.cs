using System.Collections.Generic;
using System.Threading.Tasks;
using CQRSlite.Events;

namespace Watson.Domain._Infrastructure {
    public interface IEventProjectionFeeder
    {
        Task Feed(IEvent @event);
        Task Feed(IEnumerable<IEvent> events);
    }
}