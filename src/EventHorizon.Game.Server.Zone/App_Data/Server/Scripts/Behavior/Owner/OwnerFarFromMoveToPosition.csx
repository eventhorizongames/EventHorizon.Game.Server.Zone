/// <summary>
/// Name: Behavior_Owner_OwnerFarFromMoveToPosition.csx
/// 
/// This script will check if the Actors Move To Location is "far" from their Owner's Current Location
///
/// if the owner is farther than [Actor.CompanionState.DistanceToFollow????] units
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
using EventHorizon.Zone.System.Agent.Model;
using EventHorizon.Zone.System.Agent.Model.Path;
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

        var actor = data.Get<AgentEntity>("Actor");
        var delta = 10; // TODO: Get this from Setting : DistanceToFollow * 2
        var actorPathState = actor.GetProperty<PathState>(PathState.PROPERTY_NAME);
        var actorOwnerState = actor.GetProperty<OwnerState>(OwnerState.PROPERTY_NAME);
        var owner = await GetOwner(
            services,
            actorOwnerState,
            actor
        );
        if (actorPathState.Path() == null
		    || !actorPathState.MoveTo.HasValue
        )
        {
            // Owner is not Far from their delta location.
            return new BehaviorScriptResponse(
                BehaviorNodeStatus.FAILED
            );
        }
        // This will is the position they are moving to.
        var ownerMoveTo = actorPathState.MoveTo.Value;
        var ownerCurrentPosition = owner.Transform.Position;
        // Get distance between Owner Move To and Current Position.
        var distance = Vector3.Distance(
            ownerMoveTo,
            ownerCurrentPosition
        );

        // Check still within delta
        if (distance <= delta)
        {
            // Owner is not Far from their delta location.
            return new BehaviorScriptResponse(
                BehaviorNodeStatus.FAILED
            );
        }

        // Owner is far from their delta location
        return new BehaviorScriptResponse(
            BehaviorNodeStatus.SUCCESS
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
