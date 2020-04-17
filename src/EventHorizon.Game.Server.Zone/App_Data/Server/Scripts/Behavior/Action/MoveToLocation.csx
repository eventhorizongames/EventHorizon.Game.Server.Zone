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
using EventHorizon.Zone.Core.Events.Client.Actions;
using EventHorizon.Zone.Core.Model.Client.DataType;
using EventHorizon.Zone.Core.Model.Entity;

var actor = Data.Get<IObjectEntity>("Actor");

// Check for Actor already moving
// TODO: Move this check out of here and into another Behavior Action/Condition Script
var isAgentMoving = await Services.Mediator.Send(
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
    await Services.Mediator.Send(
        new MoveAgentToPositionEvent(
            actor.Id,
            toPosition
        )
    );

    // Set the ActorMoveToPosition to "null", 
    //  this way when next update comes through it will be a 
    //  SUCCESS if agent is not moving.
    actor.SetProperty<Vector3?>(
        "ActorMoveToPosition",
        null
    );
}

return new BehaviorScriptResponse(
    BehaviorNodeStatus.RUNNING
);