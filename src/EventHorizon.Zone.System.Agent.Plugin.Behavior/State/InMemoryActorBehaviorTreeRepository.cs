namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.State
{
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.Api;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;

    using global::System.Collections.Concurrent;
    using global::System.Collections.Generic;

    public class InMemoryActorBehaviorTreeRepository : ActorBehaviorTreeRepository
    {
        private readonly ActorBehaviorTreeShape DEFAULT_TREE_SHAPE = default(ActorBehaviorTreeShape);
        private readonly ConcurrentDictionary<string, ActorBehaviorTreeShape> MAP = new ConcurrentDictionary<string, ActorBehaviorTreeShape>();

        public ActorBehaviorTreeShape FindTreeShape(
            string treeId
        )
        {
            if (MAP.TryGetValue(
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
            MAP.AddOrUpdate(
                treeId,
                behaviorTreeShape,
                (_, __) => behaviorTreeShape
            );
        }

        public IEnumerable<string> TreeIdList()
        {
            return MAP.Keys;
        }
    }
}
