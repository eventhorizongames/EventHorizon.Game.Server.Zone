using System.Collections.Generic;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Model.Entity;
using EventHorizon.Zone.System.Agent.Behavior.Api;
using EventHorizon.Zone.System.Agent.Behavior.Model;
using EventHorizon.Zone.System.Agent.Behavior.State;

namespace EventHorizon.Zone.System.Agent.Behavior.Interpreter
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
            AgentBehaviorTreeShape shape,
            IObjectEntity actor
        )
        {
            // TODO: Setup this up from the shape and the current actor state
            var treeState = new BehaviorTreeState(
                shape
            );

            do
            {
                // Run the state through the Interperters.
                treeState = await _interpreterMap.InterperterByType(
                    treeState.ActiveNode.Type
                ).Run(
                    actor,
                    treeState
                );

                // TODO: Check timming of current loop. If over timeout, stash Actor state for future.
            } while (treeState.ContainsNext);
            return treeState;
        }
    }
}