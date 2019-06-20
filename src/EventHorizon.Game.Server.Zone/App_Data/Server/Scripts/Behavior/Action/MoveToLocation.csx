/// <summary>
/// Name: Routine_Action_MoveToLocation.csx
/// 
/// This script will move the Actor to the specified location
/// 
/// Actor: { 
///     Id: long;
///     BehaviorState: IBehaviorState;
///     ActorMoveToPostion: Vector3;
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

// Check for Actor already moving
var isAgentMoving = await Services.Mediator.Send(
    new IsAgentMoving(
        Actor.Id
    )
);

// In not running startup new Move Routine from MoveTo data in Action
if (!isAgentMoving)
{
    var toPosition = Actor.GetProperty<Vector3>("ActorMoveToPostion");
    if (toPosition == null)
    {
        return new BehaviorScriptResponse(
            BehaviorNodeStatus.SUCCESS
        );
    }
    Services.Mediator.Publish(new StartAgentMoveRoutineEvent
    {
        EntityId = Actor.Id,
        ToPosition = toPosition
    }).ConfigureAwait(false);

    // Set the ActorMoveToPostion to null, 
    //  this way when next update comes through it will be a 
    //  SUCCESS if agent is not moving.
    Actor.SetProperty<string>(
        "ActorMoveToPostion",
        null
    );
}

return new BehaviorScriptResponse(
    BehaviorNodeStatus.RUNNING
);