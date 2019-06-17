using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Model.Entity;
using EventHorizon.Zone.System.Agent.Behavior.Api;
using EventHorizon.Zone.System.Agent.Behavior.Model;
using EventHorizon.Zone.System.Agent.Behavior.State;

namespace EventHorizon.Zone.System.Agent.Behavior.Interpreters
{
    /// <summary>
    /// TODO: This should short-circuit the Current Traversal to FAILURE on failed. 
    /// 
    /// Conditions check that certain actor or game world states hold true. 
    /// 
    /// If a sequence node has a condition as one of its children then the failing of the condition will prevent the following nodes from being traversed during the update. 
    /// 
    /// When placed below a concurrent node, conditions become a kind of invariant check that prevents its sibling nodes from running if a necessary state becomes invalid.
    /// </summary>
    public class ConditionInterpreter : BehaviorInterpreter
    {
        public Task<BehaviorTreeState> Run(
            IObjectEntity actor,
            BehaviorTreeState behaviorTreeState
        )
        {
            // Check if READY
            if (BehaviorNodeStatus.READY.Equals(
                behaviorTreeState.ActiveNode.Status
            ))
            {
                // When Ready run first pass
                return Task.FromResult(
                    behaviorTreeState.SetStatusOnActiveNode(
                        BehaviorNodeStatus.VISITING
                    ).SetStatusOnActiveNode(
                        // TODO: Fire the Script on the Condition
                        // The response from the script fire will fill the status
                        BehaviorNodeStatus.SUCCESS
                    ).SetTraversalToActiveNode(
                    // Set Active proccessing node back to Traversal, 
                    // This is the parent of this node, triggering validation of status.
                    )
                );
            }
            else if (BehaviorNodeStatus.RUNNING.Equals(
                behaviorTreeState.ActiveNode.Status
            ))
            {
                // When running re-run script to check state of RUNNING status
                return Task.FromResult(
                    behaviorTreeState.SetStatusOnActiveNode(
                        // TODO: Fire the Script on the Condition
                        // The response from the script fire will fill the status
                        BehaviorNodeStatus.SUCCESS
                    ).SetTraversalToActiveNode(
                    // Set Active proccessing node back to Traversal, 
                    // This is the parent of this node, triggering validation of status.
                    )
                );
            }
            // If not RUNNING/READY, reset active Node to Traversal Node.
            return Task.FromResult(
                behaviorTreeState.SetTraversalToActiveNode(
                // Set Active proccessing node back to Traversal, 
                // This is the parent of this node, triggering validation of status.
                )
            );
        }
    }
}