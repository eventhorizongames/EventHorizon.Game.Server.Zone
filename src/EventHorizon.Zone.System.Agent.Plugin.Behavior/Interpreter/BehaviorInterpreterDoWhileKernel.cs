using System.Threading.Tasks;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Api;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.State;

namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Interpreter
{
    public class BehaviorInterpreterDoWhileKernel : BehaviorInterpreterKernel
    {
        readonly BehaviorInterpreterMap _interpreterMap;
        public BehaviorInterpreterDoWhileKernel(
            BehaviorInterpreterMap interpreterMap
        )
        {
            _interpreterMap = interpreterMap;
        }
        public async Task<BehaviorTreeState> Tick(
            ActorBehaviorTreeShape shape,
            IObjectEntity actor
        )
        {
            var treeState = GetActorState(
                shape,
                actor
            ).PopActiveNodeFromQueue();

            do
            {
                // Run the state through the Interperters.
                treeState = await _interpreterMap.InterperterByType(
                    treeState.ActiveNode.Type
                ).Run(
                    actor,
                    treeState
                );

                while (treeState.CheckTraversal)
                {
                    treeState = await _interpreterMap.InterperterByType(
                        treeState.SetCheckTraversal(
                            false
                        ).ActivateNode(
                            treeState.ActiveTraversal.Token
                        ).ActiveTraversal.Type
                    ).Run(
                        actor,
                        treeState
                    );
                }
            } while (treeState.ContainsNext);
            actor.SetProperty(
                "BehaviorTreeState",
                treeState
            );
            return treeState;
        }

        private BehaviorTreeState GetActorState(
            ActorBehaviorTreeShape shape,
            IObjectEntity actor
        )
        {
            var treeState = actor.GetProperty<BehaviorTreeState>(
                "BehaviorTreeState"
            );
            if (!treeState.IsValid)
            {
                return new BehaviorTreeState(
                    shape
                );
            }
            return treeState.SetShape(
                shape
            );
        }
    }
}