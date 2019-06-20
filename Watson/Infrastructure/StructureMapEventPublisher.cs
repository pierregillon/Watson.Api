using System.Threading;
using System.Threading.Tasks;
using CQRSlite.Events;

namespace Watson.Infrastructure
{
    public class StructureMapEventPublisher : IEventPublisher
    {
        private readonly StructureMapEventProjectionFeeder _feeder;

        public StructureMapEventPublisher(StructureMapEventProjectionFeeder feeder)
        {
            _feeder = feeder;
        }

        public async Task Publish<T>(T @event, CancellationToken cancellationToken) where T : class, IEvent
        {
            ThreadPool.QueueUserWorkItem(async x => {
                await _feeder.Feed(@event);
            });

            await Task.Delay(0);
        }
    }
}