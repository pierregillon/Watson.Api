namespace try4real.ddd
{
    public interface IEventPublisher
    {
        void Publish(Event @event);
    }
}