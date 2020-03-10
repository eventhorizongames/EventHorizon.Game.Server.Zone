using System.Threading.Tasks;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Api;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.State;

namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Interpreters
{
    /// <summary>
    /// Concurrent nodes visit all of their children during each traversal. 
    ///
    /// A pre-specified number of children needs to fail to make the concurrent node fail, too.
    /// 
    /// Instead of running its child nodes truly in parallel to each other there might be a
    ///  specific traversal order which can be exploited when adding conditions(see below)
    ///  to a concurrent node because an early failing condition prevents its following
    ///  concurrent siblings from running.
    /// </summary>
    public partial class ConcurrentSelectorInterpreter : BehaviorInterpreter
    {
        public Task<BehaviorTreeState> Run(
            IObjectEntity actor,
            BehaviorTreeState behaviorTreeState
        )
        {
            behaviorTreeState = behaviorTreeState.Report(
                "Concurrent Selector Interpreter START"
            );
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
                    .Report(
                        "Concurrent Selector Interpreter EXIT"
                    )
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
                    foundFailed++;
                    if (foundFailed >= behaviorTreeState.ActiveTraversal.FailGate)
                    {
                        return Task.FromResult(
                            behaviorTreeState.SetStatusOnTraversalNode(
                                BehaviorNodeStatus.FAILED
                            ).AdvanceQueueToPassedToken(
                                behaviorTreeState.GetTokenAfterLastChildOfTraversalNode()
                            ).SetTraversalToCheck(
                            // This will make the current Traversal Node Active
                            //  and ready for processing/validation
                            ).PopActiveTraversalNode(
                            // Because of failure we pop out of the Traversal node
                            // This will make the Parent Node the Current Traversal
                            ).Report(
                                "Concurrent Selector Interpreter EXIT"
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
                    ).Report(
                        "Concurrent Selector Interpreter EXIT"
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
                    ).Report(
                        "Concurrent Selector Interpreter EXIT"
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
                ).Report(
                    "Concurrent Selector Interpreter EXIT"
                )
            );
        }
    }
}