/// <summary>
/// Routine Name: WANDER
/// 
/// Actor: { Id: long; } 
/// Data: { }
/// Services: { Mediator: IMediator; Random: IRandomNumberGenerator; DateTime: IDateTimeService; I18n: I18nLookup; }
/// </summary>

using EventHorizon.Game.Server.Zone.Agent.Ai.Model;
using EventHorizon.Game.Server.Zone.Agent.Ai.Move;
using EventHorizon.Game.Server.Zone.Events.Map;

// Get Map Nodes around Agent, within distance
var mapNodes = await Services.Mediator.Send(new GetMapNodesAroundPositionEvent
{
    Position = Actor.Position.CurrentPosition,
    Distance = Actor.GetProperty<AgentWanderState>(AgentWanderState.WANDER_NAME).LookDistance
});
if (mapNodes.Count == 0)
{
    return;
}
var randomNodeIndex = Services.Random.Next(0, mapNodes.Count);
var node = mapNodes[randomNodeIndex];

await Services.Mediator.Publish(new StartAgentMoveRoutineEvent
{
    EntityId = Actor.Id,
    ToPosition = node.Position
});