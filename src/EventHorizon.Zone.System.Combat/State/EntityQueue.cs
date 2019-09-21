using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Combat.Model.Level;

namespace EventHorizon.Zone.System.Combat.State
{
    public class EntityQueue<T> : IEntityQueue<T>
    {
        private Queue<T> _queue = new Queue<T>();

        public Task Enqueue(T entity)
        {
            _queue.Enqueue(entity);
            return Task.CompletedTask;
        }

        public Task<T> Dequeue()
        {
            if (_queue.Count == 0)
            {
                return Task.FromResult(
                    default(T)
                );
            }
            return Task.FromResult(
                _queue.Dequeue()
            );
        }
    }
}