/// <summary>
/// Name: Behavior_Action_MoveToLocation.csx
/// 
/// This script will move the Actor to the specified location
/// 
/// Actor: { 
///     Id: long;
///     BehaviorState: IBehaviorState;
///     ActorMoveToPosition: Vector3;
/// } 
/// Services: { 
///     Mediator: IMediator; 
///     Random: IRandomNumberGenerator; 
///     DateTime: IDateTimeService; 
///     I18n: I18nLookup; 
/// }
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
var isAgentMoving = await Services.Mediator.Send(
    new IsAgentMoving(
        actor.Id
    )
);

// In not running startup new Move Behavior from MoveTo data in Action
if (!isAgentMoving)
{
    var toPositionNullable = actor.GetProperty<Vector3?>("ActorMoveToPosition");
    if (!toPositionNullable.HasValue) // TODO: Find better way to check Vector3
    {
        return new BehaviorScriptResponse(
            BehaviorNodeStatus.SUCCESS
        );
    }
    var toPosition = toPositionNullable.Value;
    await Services.Mediator.Send(
        new MoveAgentToPositionEvent
        {
            AgentId = actor.Id,
            ToPosition = toPosition
        }
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