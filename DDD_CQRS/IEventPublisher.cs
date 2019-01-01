namespace fakenewsisor.server.DDD_CQRS
{
    public interface IEventPublisher
    {
        void Publish<T>(T @event) where T : Event;
    }
}