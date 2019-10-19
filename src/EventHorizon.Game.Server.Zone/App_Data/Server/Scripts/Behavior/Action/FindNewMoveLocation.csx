/// <summary>
/// Name: Behavior_Action_FindNewMoveLocation.csx
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
using EventHorizon.Zone.Core.Events.Map;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Agent.Plugin.Ai.Model;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Script;

var actor = Data.Get<IObjectEntity>("Actor");

// Get Map Nodes around Agent, within distance
var mapNodes = await Services.Mediator.Send(
    new GetMapNodesAroundPositionEvent(
        actor.Position.CurrentPosition,
        actor.GetProperty<AgentWanderState>(
            AgentWanderState.WANDER_NAME
        ).LookDistance
    )
);
if (mapNodes.Count == 0)
{
    return new BehaviorScriptResponse(
        BehaviorNodeStatus.FAILED
    );
}
var randomNodeIndex = Services.Random.Next(0, mapNodes.Count - 1);
var node = mapNodes[randomNodeIndex];

// Add MoveToNode to Actor State
actor.SetProperty<Vector3>(
    "ActorMoveToPosition",
    node.Position
// new Vector3(
//     0,0,0
// )
);

return new BehaviorScriptResponse(
    BehaviorNodeStatus.SUCCESS
);