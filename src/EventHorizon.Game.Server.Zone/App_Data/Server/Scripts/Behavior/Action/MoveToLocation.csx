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
using EventHorizon.Game.Server.Zone.Agent.Move;
using EventHorizon.Game.Server.Zone.Agent.Ai.Move;
using EventHorizon.Zone.System.Agent.Behavior.Script;
using EventHorizon.Zone.System.Agent.Behavior.Model;
using EventHorizon.Game.Server.Zone.Events.Client.Actions;
using EventHorizon.Game.Server.Zone.Client.DataType;

// Check for Actor already moving
var isAgentMoving = await Services.Mediator.Send(
    new IsAgentMoving(
        Actor.Id
    )
);

// In not running startup new Move Behavior from MoveTo data in Action
if (!isAgentMoving)
{
    var toPosition = Actor.GetProperty<Vector3>("ActorMoveToPosition");
    if (toPosition == default(Vector3))
    {
        return new BehaviorScriptResponse(
            BehaviorNodeStatus.SUCCESS
        );
    }
    Services.Mediator.Send(new MoveAgentToPosition
    {
        AgentId = Actor.Id,
        ToPosition = toPosition
    }).ConfigureAwait(false);

    // Set the ActorMoveToPosition to null, 
    //  this way when next update comes through it will be a 
    //  SUCCESS if agent is not moving.
    Actor.SetProperty<Vector3>(
        "ActorMoveToPosition",
        default(Vector3)
    );
}

return new BehaviorScriptResponse(
    BehaviorNodeStatus.RUNNING
);