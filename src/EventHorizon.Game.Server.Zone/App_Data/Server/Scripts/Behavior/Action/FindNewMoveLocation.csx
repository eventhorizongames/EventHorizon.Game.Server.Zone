/// <summary>
/// Name: Behavior_Action_FindNewMoveLocation.csx
/// 
/// This script should check the state of the 
///
/// Data: 
/// - Actor: <see cref="EventHorizon.Zone.Core.Model.Entity.IObjectEntity" />
/// Services: <see cref="EventHorizon.Zone.System.Server.Scripts.Model.ServerScriptServices" />
/// </summary>

using System.Numerics;
using EventHorizon.Zone.Core.Events.Map;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Agent.Plugin.Ai.Model;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Script;

using System.Collections.Generic;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Server.Scripts.Model;
using Microsoft.Extensions.Logging;

public class __SCRIPT__
    : ServerScript
{
    public string Id => "__SCRIPT__";
    public IEnumerable<string> Tags => new List<string> { "testing-tag" };

    public async Task<ServerScriptResponse> Run(
        ServerScriptServices services,
        ServerScriptData data
    )
    {
        var logger = services.Logger<__SCRIPT__>();
        logger.LogDebug("__SCRIPT__ - Server Script");

        var actor = data.Get<IObjectEntity>("Actor");

        // Get Map Nodes around Agent, within distance
        var mapNodes = await services.Mediator.Send(
            new GetMapNodesAroundPositionEvent(
                actor.Transform.Position,
                actor.GetProperty<AgentWanderState>(
                    AgentWanderState.WANDER_NAME
                ).LookDistance
            )
        );
        if (mapNodes.Count == 0)
        {
            return new BehaviorScriptResponse(
                BehaviorNodeStatus.FAILED
            );
        }
        var randomNodeIndex = services.Random.Next(0, mapNodes.Count - 1);
        var node = mapNodes[randomNodeIndex];

        // Add MoveToNode to Actor State
        actor.SetProperty(
            "ActorMoveToPosition",
            node.Position
        );

        return new BehaviorScriptResponse(
            BehaviorNodeStatus.SUCCESS
        );
    }
}
