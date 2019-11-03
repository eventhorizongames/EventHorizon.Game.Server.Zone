/// <summary>
/// Name: Behavior_Action_WaitSomeTime.csx
/// 
/// This script will wait a random amount of time before giving a Success.
/// 
/// Will Check for Running, 
/// 
/// If Running
///     Check state of timer
/// Else
///     Set new future TimeSpan
/// 
/// Actor: { 
///     Id: long; 
///     BehaviorState: IBehaviorState;
/// } 
/// Services: { 
///     Mediator: IMediator; 
///     Random: IRandomNumberGenerator; 
///     DateTime: IDateTimeService; 
///     I18n: I18nLookup; 
/// }
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

var actor = Data.Get<IObjectEntity>("Actor");
var distanceToRunAway = 25; // TODO: Move this to Actor State
var runToLocationDistance = 5; // TODO: Move this to Actor State
var runFromPlayerId = actor.GetProperty<long>("Behavior:RunFromPlayer.csx:RunFromPlayerId");

// Get Player for Current Position 
var player = await Services.Mediator.Send(
    new GetEntityByIdEvent
    {
        EntityId = runFromPlayerId
    }
);

// Get Direction away from Player
var positionToMove = GetPositionToMoveTo(
    player.Position.CurrentPosition,
    actor.Position.CurrentPosition,
    distanceToRunAway
);

// Send Move request to position
var node = await GetNodeEntity(
    positionToMove,
    runToLocationDistance
);
if (node == null)
{
    return new BehaviorScriptResponse(
        BehaviorNodeStatus.FAILED
    );
}

// Add MoveToNode to Actor State
actor.SetProperty<Vector3>(
    "ActorMoveToPosition",
    node.Position
);

return new BehaviorScriptResponse(
    BehaviorNodeStatus.SUCCESS
);

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
/// <param name="runToLocationDistance">How far away from positionToMove the node can be.</param>
/// <returns>The node found, otherwise null.</returns>
private async Task<IOctreeEntity> GetNodeEntity(
    Vector3 positionToMove,
    int runToLocationDistance
)
{
    var mapNodes = await Services.Mediator.Send(
        new GetMapNodesAroundPositionEvent(
            positionToMove,
            runToLocationDistance
        )
    );
    if (mapNodes.Count == 0)
    {
        return null;
    }
    return mapNodes[0];
}