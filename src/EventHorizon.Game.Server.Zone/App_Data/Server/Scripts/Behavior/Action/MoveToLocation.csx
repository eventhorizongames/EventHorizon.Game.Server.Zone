/// <summary>
/// Name: Behavior_Action_MoveToLocation.csx
/// 
/// This script will move the Actor to the Vector3 of the entity property: ActorMoveToPosition
/// 
/// Data: 
/// - Actor: <see cref="EventHorizon.Zone.Core.Model.Entity.IObjectEntity" />
/// Services: <see cref="EventHorizon.Zone.System.Server.Scripts.Model.ServerScriptServices" />
/// </summary>

using System.Numerics;
using EventHorizon.Zone.System.Agent.Events.Move;
using EventHorizon.Zone.System.Agent.Plugin.Ai.Model;
using EventHorizon.Zone.System.Agent.Plugin.Move.Events;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Script;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;
using EventHorizon.Zone.Core.Model.Entity;

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

        // Check for Actor already moving
        // TODO: Move this check out of here and into another Behavior Action/Condition Script
        var isAgentMoving = await services.Mediator.Send(
            new IsAgentMoving(
                actor.Id
            )
        );

        // In not running startup new Move Behavior from MoveTo data in Action
        if (!isAgentMoving)
        {
            var toPositionNullable = actor.GetProperty<Vector3?>("ActorMoveToPosition");
            if (!toPositionNullable.HasValue)
            {
                return new BehaviorScriptResponse(
                    BehaviorNodeStatus.SUCCESS
                );
            }
            var toPosition = toPositionNullable.Value;
            await services.Mediator.Send(
                new MoveAgentToPositionEvent(
                    actor.Id,
                    toPosition
                )
            );

            // Set the ActorMoveToPosition to "null", 
            //  this way when next update comes through it will be a 
            //  SUCCESS if agent is not moving.
            actor.SetProperty<Vector3?, IObjectEntity>(
                "ActorMoveToPosition",
                null
            );
        }

        return new BehaviorScriptResponse(
            BehaviorNodeStatus.RUNNING
        );
    }
}
