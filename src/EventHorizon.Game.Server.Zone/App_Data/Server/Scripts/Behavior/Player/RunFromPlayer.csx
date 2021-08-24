/// <summary>
/// Name: Behavior_Player_RunFromPlayer.csx
/// 
/// This script will make the actor run from any nearby players.
/// 
/// Will Check for Running, 
/// 
/// If Running
///     Check state of timer
/// Else
///     Set new future TimeSpan
/// 
/// Data:
///     Actor: <see cref="EventHorizon.Zone.Core.Model.Entity.IObjectEntity" /> 
/// Services: <see cref="EventHorizon.Zone.System.Server.Scripts.Model.ServerScriptServices"></see>
/// </summary>

using System.Numerics;
using EventHorizon.Zone.System.Agent.Plugin.Ai.Model;
using EventHorizon.Zone.Core.Events.Map;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Script;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.Core.Events.Entity.Find;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Model.Structure;
using EventHorizon.Zone.System.Agent.Plugin.Wild.Model;


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
        var wildState = actor.GetProperty<AgentWildState>(AgentWildState.PROPERTY_NAME);
        var distanceToRunAway = wildState.DistanceToRunAway;
        var deltaToPosition = wildState.DeltaDistance;
        var runFromPlayerId = actor.GetProperty<long>("Behavior:RunFromPlayer.csx:RunFromPlayerId");

        // Get Player for Current Position 
        var player = await services.Mediator.Send(
            new GetEntityByIdEvent
            {
                EntityId = runFromPlayerId
            }
        );

        // Get Direction away from Player
        var positionToMove = GetPositionToMoveTo(
            player.Transform.Position,
            actor.Transform.Position,
            distanceToRunAway
        );

        // Send Move request to position
        var node = await GetNodeEntity(
            services,
            positionToMove,
            deltaToPosition
        );
        if (node == null)
        {
            return new BehaviorScriptResponse(
                BehaviorNodeStatus.FAILED
            );
        }
        logger.LogDebug("__SCRIPT__ - Node Valid");

        // Add MoveToNode to Actor State
        actor.SetProperty(
            "ActorMoveToPosition",
            node.Position
        );

        return new BehaviorScriptResponse(
            BehaviorNodeStatus.SUCCESS
        );
    }

    /// <summary>
    /// This will get a position away from the passed in playerPosition away from the passed in actorPosition.
    /// </summary>
    /// <param name="playerPosition">The position of a player to run away from.</param>
    /// <param name="actorPosition">The actor/agent position.</param>
    /// <returns>A position away from the playerPosition to move to.</returns>
    private Vector3 GetPositionToMoveTo(
        Vector3 playerPosition,
        Vector3 actorPosition,
        int distanceToRunAway
    )
    {
        var direction = Vector3.Normalize(
            playerPosition - actorPosition
        );
        var oppositeDirection = Vector3.Multiply(
            direction,
            -1 // A -1 will cause a Vector3 to reverse the values.
        );
        return Vector3.Add(
            actorPosition,
            Vector3.Multiply(
                oppositeDirection,
                distanceToRunAway
            )
        );
    }

    /// <summary>
    /// Lookup the closest node to the positionToMove.
    /// </summary>
    /// <param name="positionToMove">Position in the world.</param>
    /// <param name="deltaToPosition">How far away from positionToMove the node can be.</param>
    /// <returns>The node found, otherwise null.</returns>
    private async Task<IOctreeEntity> GetNodeEntity(
        ServerScriptServices services,
        Vector3 positionToMove,
        int deltaToPosition
    )
    {
        var mapNodes = await services.Mediator.Send(
            new GetMapNodesAroundPositionEvent(
                positionToMove,
                deltaToPosition
            )
        );
        if (mapNodes.Count == 0)
        {
            return null;
        }
        return mapNodes[0];
    }
}
