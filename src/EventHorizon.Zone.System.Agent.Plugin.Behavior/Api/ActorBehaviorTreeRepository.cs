namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Api
{
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;

    using global::System.Collections.Generic;

    public interface ActorBehaviorTreeRepository
    {
        void RegisterTree(
            string treeId,
            ActorBehaviorTreeShape behaviorTreeShape
        );
        ActorBehaviorTreeShape FindTreeShape(
            string treeId
        );
        IEnumerable<string> TreeIdList();
    }
}
