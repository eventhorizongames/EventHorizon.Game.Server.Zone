namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Api
{
    using global::System.Collections.Generic;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;

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