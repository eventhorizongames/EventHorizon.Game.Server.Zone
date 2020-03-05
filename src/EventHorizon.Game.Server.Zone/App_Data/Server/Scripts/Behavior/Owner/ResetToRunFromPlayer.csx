/// <summary>
/// Name: Behavior_Owner_ResetToRunFromPlayer.csx
/// 
/// Will Set the Current Behavior Tree to RunFromPlayer
/// Will Reset the State of the Actor Owner to Empty string
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

using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Agent.Events.Update;
using EventHorizon.Zone.System.Agent.Model;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Change;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Script;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.State;
using EventHorizon.Zone.System.Agent.Plugin.Companion.Model;

System.Console.WriteLine("ResetToRunFromPlayer");
var resetScriptId = "Behaviors_Idle.json";
// TODO: Uncomment to get real behavior. Get Settings from Actor : Reset Behavior Id
// var resetScriptId = "Behaviors_RunFromPlayer.json";
var actor = Data.Get<AgentEntity>("Actor");
var agentBehaviorState = actor.GetProperty<AgentBehavior>(AgentBehavior.PROPERTY_NAME);
var ownerState = actor.GetProperty<OwnerState>(OwnerState.PROPERTY_NAME);

ownerState.CanBeCaptured = true;
ownerState.OwnerId = string.Empty;

actor.SetProperty(
    OwnerState.PROPERTY_NAME,
    ownerState
);
// Behaviors_FollowOwner_WIP.json
await Services.Mediator.Send(
    new AgentUpdateEntityCommand(
        actor,
        AgentAction.SCRIPT
    )
);

await Services.Mediator.Send(
    new ChangeActorBehaviorTreeCommand(
        actor,
        resetScriptId
    )
);

return new BehaviorScriptResponse(
    BehaviorNodeStatus.SUCCESS
);