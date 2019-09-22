using System.Threading.Tasks;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Api;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.State;

namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Interpreters
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
            if (behaviorTreeState.ContainedInLastTraversal(
                behaviorTreeState.ActiveNode.Token
            ))
            {
                behaviorTreeState = behaviorTreeState
                    .SetStatusOnActiveNode(
                        BehaviorNodeStatus.VISITING
                    )
                    .RemoveNodeFromLastTraversalStack(
                        behaviorTreeState.ActiveNode.Token
                    )
                    .PushActiveNodeToTraversalStack()
                    .PopActiveNodeFromQueue();
                while (
                    behaviorTreeState.IsActiveNodeValidAndNotRunning()
                )
                {
                    behaviorTreeState = behaviorTreeState
                        .SetStatusOnActiveNode(
                            BehaviorNodeStatus.SUCCESS
                        ).PopActiveNodeFromQueue();
                }
                return Task.FromResult(
                    behaviorTreeState
                );
            }

            if (BehaviorNodeStatus.READY.Equals(
                behaviorTreeState.ActiveNode.Status
            ))
            {
                return Task.FromResult(
                    behaviorTreeState.SetStatusOnActiveNode(
                        BehaviorNodeStatus.VISITING
                    ).PushActiveNodeToTraversalStack()
                    .SetNextActiveNode()
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
                        ).SetTraversalToCheck(
                        // This will make the current Traversal Node Active
                        //  and ready for processing/validation
                        ).PopActiveTraversalNode(
                        // Pop the current node from the stack.
                        // Since it failed it will now need to be processed by 
                        //  this nodes parent.
                        )
                    );
                }
                else if (BehaviorNodeStatus.ERROR.Equals(
                    childNode.Status
                ))
                {
                    return Task.FromResult(
                        behaviorTreeState.SetStatusOnTraversalNode(
                            BehaviorNodeStatus.ERROR
                        ).SetTraversalToCheck(
                        // This will make the current Traversal Node Active
                        //  and ready for processing/validation
                        ).PopActiveTraversalNode(
                        // Pop the current node from the stack.
                        // Since it failed it will now need to be processed by 
                        //  this nodes parent.
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
                        ).SetTraversalToCheck(
                        // This will make the current Traversal Node Active
                        //  and ready for processing/validation
                        ).PopActiveTraversalNode(
                        // This will pop the current Active Traversal off the stack
                        // This will allow for the Parent node to run processing.
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
                ).SetTraversalToCheck(
                // This will make the current Traversal Node Active
                //  and ready for processing/validation
                ).PopActiveTraversalNode(
                // This will pop the current Active Traversal off the stack
                )
            );
        }
    }
}