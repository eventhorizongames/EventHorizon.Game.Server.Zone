namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Interpreters;

using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Api;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Script;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Script.Run;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.State;

using global::System;
using global::System.Threading.Tasks;

using MediatR;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

/// <summary>
/// This will short-circuit the Current Traversal to FAILURE on failed script run. 
/// 
/// Conditions check that certain actor or game world states hold true. 
/// 
/// If a sequence node has a condition as one of its children then the failing of the condition will prevent the following nodes from being traversed during the update. 
/// 
/// When placed below a concurrent node, conditions become a kind of invariant check that prevents its sibling nodes from running if a necessary state becomes invalid.
/// </summary>
public class ConditionInterpreter
    : ConditionBehaviorInterpreter
{
    private static readonly BehaviorScriptResponse FAILED_RESPONSE = new(
        BehaviorNodeStatus.FAILED
    );

    private readonly ILogger _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public ConditionInterpreter(
        ILogger<ConditionInterpreter> logger,
        IServiceScopeFactory serviceScopeFactory
    )
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task<BehaviorTreeState> Run(
        IObjectEntity actor,
        BehaviorTreeState behaviorTreeState
    )
    {
        behaviorTreeState = behaviorTreeState.Report(
            "Condition Interpreter START",
            new { behaviorTreeState.ActiveNode }
        );
        if (CheckIfStatusReadyOrRunning(
            behaviorTreeState.ActiveNode.Status
        ))
        {
            var result = await RunScript(
                actor,
                behaviorTreeState.ActiveNode.Fire
            );

            if (BehaviorNodeStatus.SUCCESS.Equals(
                result.Status
            ) || BehaviorNodeStatus.RUNNING.Equals(
                result.Status
            ))
            {
                return behaviorTreeState
                    .SetStatusOnActiveNode(
                        result.Status
                    ).SetTraversalToCheck(
                    // Set Active processing node back to Traversal, 
                    // This is the parent of this node, triggering validation of status.
                    ).Report(
                        "Condition Interpreter EXIT"
                    );
            }

            return behaviorTreeState
                .SetStatusOnActiveNode(
                    BehaviorNodeStatus.FAILED
                ).SetStatusOnTraversalNode(
                    BehaviorNodeStatus.FAILED
                ).SetTraversalToCheck(
                // Set Active processing node back to Traversal, 
                // This is the parent of this node, triggering validation of status.
                ).Report(
                    "Condition Interpreter EXIT"
                );
        }
        // If not READY/RUNNING, reset active Node to Traversal Node.
        return behaviorTreeState
            .SetTraversalToCheck(
            // Set Active processing node back to Traversal, 
            // This is the parent of this node, triggering validation of status.
            ).Report(
                "Condition Interpreter EXIT"
            );
    }

    private async Task<BehaviorScriptResponse> RunScript(
        IObjectEntity actor,
        string script
    )
    {
        try
        {
            using var serviceScope = _serviceScopeFactory.CreateScope();
            var mediator = serviceScope.ServiceProvider.GetRequiredService<IMediator>();
            return await mediator.Send(
                new RunBehaviorScript(
                    actor,
                    script
                )
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Exception during Run of Action"
            );
            return FAILED_RESPONSE;
        }
    }

    private bool CheckIfStatusReadyOrRunning(
        string? status
    ) => BehaviorNodeStatus.READY.Equals(
        status
    ) || BehaviorNodeStatus.RUNNING.Equals(
        status
    );
}
