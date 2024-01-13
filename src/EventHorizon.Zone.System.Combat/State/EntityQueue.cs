namespace EventHorizon.Zone.System.Combat.State;

using global::System.Collections.Generic;
using global::System.Threading.Tasks;

public class EntityQueue<T>
    : IEntityQueue<T>
{
    private readonly Queue<T> _queue = new();

    public Task Enqueue(T entity)
    {
        _queue.Enqueue(entity);
        return Task.CompletedTask;
    }

    public Task<T?> Dequeue()
    {
        if (_queue.Count == 0)
        {
            return Task.FromResult(
                default(T)
            );
        }
        return Task.FromResult(
            _queue.Dequeue()
        )!;
    }
}
