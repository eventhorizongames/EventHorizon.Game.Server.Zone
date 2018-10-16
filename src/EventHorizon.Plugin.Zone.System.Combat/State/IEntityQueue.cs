using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventHorizon.Plugin.Zone.System.Combat.State
{
    public interface IEntityQueue<T>
    {
        Task Enqueue(T entity);
        Task<T> Dequeue();
    }
}