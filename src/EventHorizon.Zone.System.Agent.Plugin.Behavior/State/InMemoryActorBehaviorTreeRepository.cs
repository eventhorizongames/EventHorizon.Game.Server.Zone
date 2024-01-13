namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.State;

using EventHorizon.Zone.System.Agent.Plugin.Behavior.Api;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;

using global::System.Collections.Concurrent;
using global::System.Collections.Generic;

public class InMemoryActorBehaviorTreeRepository : ActorBehaviorTreeRepository
{
    private static readonly ActorBehaviorTreeShape DEFAULT_TREE_SHAPE = default;
    private readonly ConcurrentDictionary<string, ActorBehaviorTreeShape> _map = new();

    public ActorBehaviorTreeShape FindTreeShape(
        string treeId
    )
    {
        if (_map.TryGetValue(
            treeId,
            out var shape
        ))
        {
            return shape;
        }
        return DEFAULT_TREE_SHAPE;
    }

    public void RegisterTree(
        string treeId,
        ActorBehaviorTreeShape behaviorTreeShape
    )
    {
        _map.AddOrUpdate(
            treeId,
            behaviorTreeShape,
            (_, _) => behaviorTreeShape
        );
    }

    public IEnumerable<string> TreeIdList()
    {
        return _map.Keys;
    }
}
