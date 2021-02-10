/// <summary>
/// Name: Behavior_Owner_IsFarFromOwner.csx
/// 
/// This script will find the owner location and compare it to the actor/agent location.
///
/// if the owner is farther than 5 units
///  it will move to that location
/// if not
///  it will fail the check
/// 
/// Data:
///     Actor: <see cref="EventHorizon.Zone.Core.Model.Entity.IObjectEntity" /> 
/// Services: <see cref="EventHorizon.Zone.System.Server.Scripts.Model.ServerScriptServices"></see>
/// </summary>

using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.Entity.Find;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Script;
using EventHorizon.Zone.System.Agent.Plugin.Companion.Model;

using System.Collections.Generic;
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
        var owner = await GetOwner(
            services,
            ownerState,
            actor
        );

        // Get distance between actor to owner
        var distance = Vector3.Distance(
            actor.Transform.Position,
            owner.Transform.Position
        );

        if (distance >= ownerFollowDistance)
        {
            // They are far from their owner
            return new BehaviorScriptResponse(
                BehaviorNodeStatus.SUCCESS
            );
        }

        // They are not far from their owner
        return new BehaviorScriptResponse(
            BehaviorNodeStatus.FAILED
        );
    }

    public async Task<IObjectEntity> GetOwner(
        ServerScriptServices services,
        OwnerState ownerState,
        IObjectEntity actor
    )
    {
        // Get Current Owner of Actor of type Player
        var ownerList = await services.Mediator.Send(
            new QueryForEntities
            {
                Query = (
                    entity => entity.Type == EntityType.PLAYER
                    && entity.GlobalId == ownerState.OwnerId
                )
            }
        );
        return ownerList.FirstOrDefault();
    }
}
