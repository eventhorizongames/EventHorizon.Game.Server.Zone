using System;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Agent.Behavior.Api;
using EventHorizon.Zone.System.Agent.Behavior.Model;
using EventHorizon.Zone.System.Agent.Behavior.Script.Run;
using EventHorizon.Zone.System.Agent.Behavior.State;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EventHorizon.Zone.System.Agent.Behavior.Interpreters
{
    /// <summary>
    /// Actions which finally implement an actors or game world state changes, for example to plan a path and move on it, to sense for the nearest enemies, to show certain animations, switch weapons, or run a specified sound. 
    /// 
    /// Actions will typically coordinate and call into different game systems. 
    /// 
    /// They might run for one simulation tick – one frame – or might need to be ticked for multiple frames to finish their work.
    /// </summary>
    public class ActionInterpreter : ActionBehaviorInterpreter
    {
        readonly ILogger _logger;
        readonly IServiceScopeFactory _serviceScopeFactory;
        public ActionInterpreter(
            ILogger<ActionInterpreter> logger,
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
            if (BehaviorNodeStatus.READY.Equals(
                behaviorTreeState.ActiveNode.Status
            ) || BehaviorNodeStatus.RUNNING.Equals(
                behaviorTreeState.ActiveNode.Status
            ))
            {
                try
                {
                    using (var serviceScope = _serviceScopeFactory.CreateScope())
                    {
                        var mediator = serviceScope.ServiceProvider.GetService<IMediator>();
                        // When Ready run first pass
                        return behaviorTreeState.SetStatusOnActiveNode(
                            BehaviorNodeStatus.VISITING
                        ).SetStatusOnActiveNode(
                            // The response from the script fire will fill the status
                            //  of the Active Node
                            (await mediator.Send(
                                new RunBehaviorScript(
                                    actor,
                                    behaviorTreeState.ActiveNode.Fire
                                )
                            )).Status
                        ).SetTraversalToCheck();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        "Exception during Run of Action",
                        ex
                    );
                    return behaviorTreeState.SetStatusOnActiveNode(
                        BehaviorNodeStatus.FAILED
                    ).SetTraversalToCheck();
                }
            }
            return behaviorTreeState.SetTraversalToCheck();
        }
    }
}