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
        // Get Owner
        var ownerState = actor.GetProperty<OwnerState>(OwnerState.PROPERTY_NAME);
        var ownerFollowDistance = ownerState.OwnerFollowDistance;
        // Get Current Owner of Actor of type Player
        var ownerList = await services.Mediator.Send(
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
        var mapNodes = await services.Mediator.Send(
            new GetMapNodesAroundPositionEvent(
                owner.Transform.Position,
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
        var randomNodeIndex = services.Random.Next(
            0,
            mapNodes.Count - 1
        );
        var node = mapNodes[randomNodeIndex];

        // Set Some temp data on this entity
        // Does not need to save the entity, since this is only used in the BT.
        actor.SetProperty(
            "ActorMoveToPosition",
            node.Position
        );

        return new BehaviorScriptResponse(
            BehaviorNodeStatus.SUCCESS
        );
    }
}
