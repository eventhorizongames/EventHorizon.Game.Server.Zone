/// <summary>
/// Name: Behavior_Player_CheckForPlayersInUpdateDistance.csx
/// 
/// This script can be used to validate that the current Actor is in 
///  distance of any players based on an Update Distance value.
/// 
/// The main purpose of this script is to return success/fail so the BT procssing
///  should continue the Actors processing.
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

// TODO: Do some logic. ;)

return new BehaviorScriptResponse(
    BehaviorNodeStatus.RUNNING
);