namespace EventHorizon.Zone.System.Agent.Move.Repository;

using EventHorizon.Zone.System.Agent.Model.State;

using global::System.Collections.Concurrent;

public class MoveAgentRepository
    : IMoveAgentRepository
{
    private readonly ConcurrentQueue<long> _entities = new();
    private readonly ConcurrentQueue<long> _toRegister = new();

    public void Register(
        long entityId
    )
    {
        _toRegister.Enqueue(
            entityId
        );
    }

    public bool Dequeue(
        out long entityId
    )
    {
        return _entities.TryDequeue(
            out entityId
        );
    }

    public void MergeRegisteredIntoQueue()
    {
        while (_toRegister.TryDequeue(
            out long entityId
        ))
        {
            _entities.Enqueue(
                entityId
            );
        }
    }
}
