using System.Linq;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Model.Entity;
using EventHorizon.Zone.System.Agent.Behavior.Api;
using EventHorizon.Zone.System.Agent.Behavior.Model;
using EventHorizon.Zone.System.Agent.Behavior.State;

namespace EventHorizon.Zone.System.Agent.Behavior.Interpreters
{
    /// <summary>
    /// Concurrent nodes visit all of their children during each traversal. 
    ///
    /// A pre-specified number of children needs to fail to make the concurrent node fail, too.
    /// 
    /// Instead of running its child nodes truly in parallel to each other there might be a specific traversal order which can be exploited when adding conditions(see below) to a concurrent node because an early failing condition prevents its following concurrent siblings from running.
    /// </summary>
    public partial class ConcurrentSelectorInterpreter : BehaviorInterpreter
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
                    .SetNextActiveNode()
                );
            }

            // Not first visit
            // Check the state of Child Nodes
            var foundFailed = 0;
            var foundRunning = 0;
            var foundReady = 0;
            foreach (var childNodeToken in behaviorTreeState.GetActiveTraversalChildren())
            {
                var childNode = behaviorTreeState.GetNode(
                    childNodeToken.Token
                );

                if (BehaviorNodeStatus.FAILED.Equals(
                    childNode.Status
                ))
                {
                    // TODO: on move to base class this will be
                    //  ConcurrentSelectorInterpreter specific behavior.
                    foundFailed++;
                    if (foundFailed >= behaviorTreeState.ActiveTraversal.FailGate)
                    {
                        return Task.FromResult(
                            behaviorTreeState.SetStatusOnTraversalNode(
                                BehaviorNodeStatus.FAILED
                            ).SetTraversalToCheck(
                            // This will make the current Traversal Node Active
                            //  and ready for processing/validation
                            ).PopActiveTraversalNode(
                            // Because of failure we pop out of the Traversal node
                            // This will make the Parent Node the Current Traversal
                            ).AdvanceQueueToAfterPassedToken(
                                behaviorTreeState.GetActiveTraversalChildren()
                                    .Last()
                                    .Token
                            )
                        );
                    }
                }
                else if (BehaviorNodeStatus.RUNNING.Equals(
                    childNode.Status
                ))
                {
                    foundRunning++;
                }
                else if (BehaviorNodeStatus.READY.Equals(
                    childNode.Status
                ))
                {
                    foundReady++;
                }
            }

            // Check for any nodes still ready to run
            if (foundReady > 0)
            {
                // If any found still in ready, run them.
                return Task.FromResult(
                    behaviorTreeState.SetNextActiveNode(
                    // This will set the next node to be processed.
                    // In this case should be the next Child
                    )
                );
            }
            // If no Nodes found to ready to run, check if any are running.
            else if (foundRunning > 0)
            {
                // If found any running update Active Traversal state.
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