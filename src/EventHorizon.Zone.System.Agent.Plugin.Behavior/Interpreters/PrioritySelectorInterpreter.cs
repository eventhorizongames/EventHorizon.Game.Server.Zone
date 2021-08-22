namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Interpreters
{
    using global::System.Threading.Tasks;

    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.Api;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.State;

    /// <summary>
    /// On each traversal priority selectors check which child to run in priority order until the first one succeeds or returns that it is running. 
    /// 
    /// One option is to call the last still running node again during the next behavior tree update. 
    /// 
    /// The other option is to always restart traversal from the highest priority child and implicitly cancel the last running child behavior if it isn’t chosen immediately again.
    /// </summary>
    public class PrioritySelectorInterpreter : BehaviorInterpreter
    {
        public Task<BehaviorTreeState> Run(
            IObjectEntity actor,
            BehaviorTreeState behaviorTreeState
        )
        {
            behaviorTreeState = behaviorTreeState.Report(
                "Priority Selector Interpreter START"
            );
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
                    .PushActiveNodeToTraversalStack();
                if (behaviorTreeState.ActiveNode.Reset)
                {
                    return Task.FromResult(
                        behaviorTreeState
                            .Report(
                                "Priority Selector Interpreter EXIT"
                            )
                    );
                }
                behaviorTreeState = behaviorTreeState
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
                        .Report(
                            "Priority Selector Interpreter EXIT"
                        )
                );
            }

            if (BehaviorNodeStatus.READY.Equals(
                behaviorTreeState.ActiveNode.Status
            ))
            {
                return Task.FromResult(
                    behaviorTreeState.SetStatusOnActiveNode(
                        BehaviorNodeStatus.VISITING
                    )
                    .PushActiveNodeToTraversalStack()
                    .SetNextActiveNode()
                    .Report(
                        "Priority Selector Interpreter EXIT"
                    )
                );
            }

            foreach (var childNodeToken in behaviorTreeState.GetActiveTraversalChildren())
            {
                var childNode = behaviorTreeState.GetNode(
                    childNodeToken.Token
                );
                if (BehaviorNodeStatus.RUNNING.Equals(
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
                        //  and ready for processing and validation
                        ).PopActiveTraversalNode(
                        // This will pop the current Active Traversal off the stack
                        // This will allow for the Parent node to run processing.
                        ).Report(
                            "Priority Selector Interpreter EXIT"
                        )
                    );
                }
                else if (BehaviorNodeStatus.SUCCESS.Equals(
                    childNode.Status
                ))
                {
                    return Task.FromResult(
                        behaviorTreeState.SetStatusOnTraversalNode(
                            // Set to Running state so can be picked up latter for 
                            //  validation at a later time.
                            BehaviorNodeStatus.SUCCESS
                        ).SetTraversalToCheck(
                        // This will make the current Traversal Node Active
                        //  and ready for processing and validation
                        ).PopActiveTraversalNode(
                        // This will pop the current Active Traversal off the stack
                        // This will allow for the Parent node to run processing.
                        ).Report(
                            "Priority Selector Interpreter EXIT"
                        )
                    );
                }
                else if (BehaviorNodeStatus.READY.Equals(
                    childNode.Status
                ) || BehaviorNodeStatus.VISITING.Equals(
                    childNode.Status
                ))
                {
                    return Task.FromResult(
                        behaviorTreeState.SetNextActiveNode(
                        // This will set the next child node, in this Traversal node,
                        //  to be processed.
                        ).Report(
                            "Priority Selector Interpreter EXIT"
                        )
                    );
                }
            }

            // If made it here, something went wrong, set status to ERROR and check traversal
            return Task.FromResult(
                behaviorTreeState.SetStatusOnTraversalNode(
                    BehaviorNodeStatus.ERROR
                ).SetTraversalToCheck(
                // This will make the current Traversal Node Active
                //  and ready for processing/validation
                ).PopActiveTraversalNode(
                // This will pop the current Active Traversal off the stack
                ).Report(
                    "Priority Selector Interpreter EXIT"
                )
            );
        }
    }
}
