/// <summary>
/// Name: Routine_Action_FindNewMoveLocation.csx
/// 
/// This script should check the state of the 
/// 
/// Actor: { 
///     Id: long; 
///     BehaviorState: IBehaviorState;
/// } 
/// Services: { 
///     Mediator: IMediator; 
///     Random: IRandomNumberGenerator; 
///     DateTime: IDateTimeService; 
///     I18n: I18nLookup; 
/// }
/// </summary>

using System.Numerics;
using EventHorizon.Game.Server.Zone.Agent.Ai.Model;
using EventHorizon.Game.Server.Zone.Events.Map;
using EventHorizon.Zone.System.Agent.Behavior.Script;
using EventHorizon.Zone.System.Agent.Behavior.Model;

// Get Map Nodes around Agent, within distance
var mapNodes = await Services.Mediator.Send(new GetMapNodesAroundPositionEvent
{
    Position = Actor.Position.CurrentPosition,
    Distance = Actor.GetProperty<AgentWanderState>(AgentWanderState.WANDER_NAME).LookDistance
});
if (mapNodes.Count == 0)
{
    return new BehaviorScriptResponse(
        BehaviorNodeStatus.FAILED
    );
}
var randomNodeIndex = Services.Random.Next(0, mapNodes.Count);
var node = mapNodes[randomNodeIndex];

// Add MoveToNode to Actor State
Actor.SetProperty<Vector3>(
    "ActorMoveToPostion", 
    node.Position
    // new Vector3(
    //     0,0,0
    // )
);

return new BehaviorScriptResponse(
    BehaviorNodeStatus.SUCCESS
);