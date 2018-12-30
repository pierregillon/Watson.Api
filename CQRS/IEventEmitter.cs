using System.Threading.Tasks;

namespace fakenewsisor.server
{
    public interface IEventEmitter
    {
        void Emit<T>(T @event);
    }
}