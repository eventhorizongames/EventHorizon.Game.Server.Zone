using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Model.Entity;
using EventHorizon.Zone.System.Agent.Behavior.Api;
using EventHorizon.Zone.System.Agent.Behavior.Model;
using EventHorizon.Zone.System.Agent.Behavior.Script.Run;
using EventHorizon.Zone.System.Agent.Behavior.State;
using MediatR;

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
        readonly IMediator _mediator;
        public ActionInterpreter(
            IMediator mediator
        )
        {
            _mediator = mediator;
        }
        public async Task<BehaviorTreeState> Run(
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
                return behaviorTreeState.SetStatusOnActiveNode(
                    BehaviorNodeStatus.VISITING
                ).SetStatusOnActiveNode(
                    // The response from the script fire will fill the status
                    //  of the Active Node
                    (await _mediator.Send(
                        new RunBehaviorScript(
                            actor,
                            behaviorTreeState.ActiveNode.Fire
                        )
                    )).Status
                ).SetTraversalToActiveNode(
                // Set Active proccessing node back to Traversal, 
                // This is the parent of this node, triggering validation of status.
                );
            }
            else if (BehaviorNodeStatus.RUNNING.Equals(
                behaviorTreeState.ActiveNode.Status
            ))
            {
                // When running re-run script to check state of RUNNING status
                return behaviorTreeState.SetStatusOnActiveNode(
                    // TODO: Fire the Script on the Condition
                    // The response from the script fire will fill the status
                    BehaviorNodeStatus.SUCCESS
                ).SetTraversalToActiveNode(
                // Set Active proccessing node back to Traversal, 
                // This is the parent of this node, triggering validation of status.
                );
            }
            // If not RUNNING/READY, reset active Node to Traversal Node.
            return behaviorTreeState.SetTraversalToActiveNode(
            // Set Active proccessing node back to Traversal, 
            // This is the parent of this node, triggering validation of status.
            );
        }
    }
}