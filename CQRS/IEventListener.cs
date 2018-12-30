using System.Threading.Tasks;

namespace fakenewsisor.server
{
    public interface IEventListener<in T>
    {
        Task On(T @event);
    }
}