/// <summary>
/// Name: Behavior_Owner_FindNewMoveLocationAroundOwner.csx
/// 
/// This script will find the Owner position and choose a location near to move.
///
/// Data:
///     Actor: <see cref="EventHorizon.Zone.Core.Model.Entity.IObjectEntity" /> 
/// Services: <see cref="EventHorizon.Zone.System.Server.Scripts.Model.ServerScriptServices"></see>
/// </summary>

using System.Linq;
using System.Numerics;
using EventHorizon.Zone.Core.Events.Entity.Find;
using EventHorizon.Zone.Core.Events.Map;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Agent.Plugin.Ai.Model;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Script;
using EventHorizon.Zone.System.Agent.Plugin.Companion.Model;

var actor = Data.Get<IObjectEntity>("Actor");
// Get Owner
var ownerState = actor.GetProperty<OwnerState>(OwnerState.PROPERTY_NAME);
var ownerFollowDistance = 5; // TODO: Get this from Actor setting : Close To Owner Distance
// Get Current Owner of Actor of type Player
var ownerList = await Services.Mediator.Send(
    new QueryForEntities
    {
        // Entity Must be a Player and have a Global Id same as the Owner's
        Query = (
            entity => entity.Type == EntityType.PLAYER
            && entity.GlobalId == ownerState.OwnerId
        )
    }
);
// Get the First Item in the list
var owner = ownerList.FirstOrDefault();

// Get Map Nodes around Owner Position, within distance
var mapNodes = await Services.Mediator.Send(
    new GetMapNodesAroundPositionEvent(
        owner.Position.CurrentPosition,
        ownerFollowDistance
    )
);
// If nothing is found, just fail the script.
if (mapNodes.Count == 0)
{
    return new BehaviorScriptResponse(
        BehaviorNodeStatus.FAILED
    );
}
// Find a Random Id using the found nodes around the Owner.
var randomNodeIndex = Services.Random.Next(
    0,
    mapNodes.Count - 1
);
var node = mapNodes[randomNodeIndex];

// Set Some temp data on this entity
// Does not need to save the entity, since this is only used in the BT.
actor.SetProperty<Vector3>(
    "ActorMoveToPosition",
    node.Position
);

return new BehaviorScriptResponse(
    BehaviorNodeStatus.SUCCESS
);