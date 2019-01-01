namespace fakenewsisor.server.DDD_CQRS
{
    public interface IEventPublisher
    {
        void Publish(Event @event);
    }
}