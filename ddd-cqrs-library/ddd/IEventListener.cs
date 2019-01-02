using System.Threading.Tasks;

namespace try4real.ddd
{
    public interface IEventListener<in T>
    {
        void On(T @event);
    }
}