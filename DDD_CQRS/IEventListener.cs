using System.Threading.Tasks;

namespace fakenewsisor.server.DDD_CQRS
{
    public interface IEventListener<in T>
    {
        void On(T @event);
    }
}