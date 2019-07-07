using System.Collections.Generic;
using EventHorizon.Zone.System.Agent.Behavior.Model;

namespace EventHorizon.Zone.System.Agent.Behavior.Api
{
    public interface ActorBehaviorTreeRepository
    {
        void ClearTrees();
        void RegisterTree(
            string treeId,
            ActorBehaviorTreeShape behaviorTreeShape
        );
        ActorBehaviorTreeShape FindTreeShape(
            string treeId
        );
        void RegisterActorToTree(
            long actorId, 
            string treeId
        );
        void UnRegisterActorFromTree(
            long actorId, 
            string treeId
        );
        IEnumerable<string> TreeIdList();
        IEnumerable<long> ActorIdList(
            string treeId
        );
    }
}