using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Model.Entity;
using EventHorizon.Zone.System.Agent.Behavior.Api;
using EventHorizon.Zone.System.Agent.Behavior.Model;
using EventHorizon.Zone.System.Agent.Behavior.State;

namespace EventHorizon.Zone.System.Agent.Behavior.Interpreters
{
    /// <summary>
    /// Sequence selectors run one child to finish after the other.
    /// 
    /// If one or multiple fail the whole sequence fails, too. 
    /// 
    /// Without a reset or without finishing the last child node a sequence stores the last running child to immediately return to it on the next update.
    /// </summary>
    public class SequenceSelectorInterpreter : BehaviorInterpreter
    {
        public Task<BehaviorTreeState> Run(
            IObjectEntity actor,
            BehaviorTreeState behaviorTreeState
        )
        {
            if (BehaviorNodeStatus.READY.Equals(
                behaviorTreeState.ActiveNode.Status
            ))
            {
                // First visit, set child node as Active Node
                return Task.FromResult(
                    behaviorTreeState.SetStatusOnActiveNode(
                        BehaviorNodeStatus.VISITING
                    ).PushActiveNodeToTraversalStack()
                    .SetNextActiveNode(
                    )
                );
            }
            if (BehaviorNodeStatus.RUNNING.Equals(
                behaviorTreeState.ActiveNode.Status
            ))
            {
                // Since RUNNING, set to VISITING 
                //  and set first child node of RUNNING to Active node.
                return Task.FromResult(
                    behaviorTreeState.SetStatusOnActiveNode(
                        BehaviorNodeStatus.VISITING
                    ).PushActiveNodeToTraversalStack()
                    .SetNextRunningNodeToActive()
                );
            }

            foreach (var childNodeToken in behaviorTreeState.GetActiveTraversalChildren())
            {
                var childNode = behaviorTreeState.GetNode(
                    childNodeToken.Token
                );

                if (BehaviorNodeStatus.FAILED.Equals(
                    childNode.Status
                ))
                {
                    return Task.FromResult(
                        behaviorTreeState.SetStatusOnTraversalNode(
                            BehaviorNodeStatus.FAILED
                        ).PopActiveTraversalNode(
                        // Pop the current node from the stack.
                        // Since it failed it will now need tobe processed by 
                        //  this nodes parent.
                        )
                        .SetTraversalToActiveNode(
                        // This will make the current Traversal Node Active
                        //  and ready for processing/validation
                        )
                    );
                }
                else if (BehaviorNodeStatus.RUNNING.Equals(
                    childNode.Status
                ))
                {
                    return Task.FromResult(
                        behaviorTreeState.SetStatusOnTraversalNode(
                            // Set to Running state so can be picked up latter for 
                            //  validation at a later time.
                            BehaviorNodeStatus.RUNNING
                        ).AddActiveTraversalToNextStack(
                        // This add the Active Traversal to the Next Stack State
                        // The next stack state will be used in next tick/update calls
                        ).PopActiveTraversalNode(
                        // This will pop the current Active Traversal off the stack
                        // This will allow for the Parent node to run processing.
                        ).SetTraversalToActiveNode(
                        // This will make the current Traversal Node Active
                        //  and ready for processing and validation
                        )
                    );
                }
                else if (BehaviorNodeStatus.READY.Equals(
                    childNode.Status
                ))
                {
                    return Task.FromResult(
                        behaviorTreeState.SetNextActiveNode(
                        // This will set the next child node, in this Traversal node,
                        //  to be processed.
                        )
                    );
                }
            }

            // If made it here they all nodes are SUCCESSFUL
            return Task.FromResult(
                behaviorTreeState.SetStatusOnTraversalNode(
                    BehaviorNodeStatus.SUCCESS
                ).PopActiveTraversalNode(
                // This will pop the current Active Traversal off the stack
                ).SetTraversalToActiveNode(
                // This will make the current Traversal Node Active
                //  and ready for processing/validation
                )
            );
        }
    }
}