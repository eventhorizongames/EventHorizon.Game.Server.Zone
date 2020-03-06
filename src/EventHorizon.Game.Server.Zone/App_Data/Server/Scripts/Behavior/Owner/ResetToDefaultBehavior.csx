/// <summary>
/// Name: Behavior_Owner_ResetToRunFromPlayer.csx
/// 
/// Will Set the Current Behavior Tree to RunFromPlayer
/// Will Reset the State of the Actor Owner to Empty string
/// 
/// Data:
///     Actor: <see cref="EventHorizon.Zone.Core.Model.Entity.IObjectEntity" /> 
/// Services: <see cref="EventHorizon.Zone.System.Server.Scripts.Model.ServerScriptServices"></see>
/// </summary>

using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Agent.Events.Update;
using EventHorizon.Zone.System.Agent.Model;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Change;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Script;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.State;
using EventHorizon.Zone.System.Agent.Plugin.Companion.Model;

System.Console.WriteLine("Reset To Default Behavior Tree");
var actor = Data.Get<AgentEntity>("Actor");
var agentBehaviorState = actor.GetProperty<AgentBehavior>(AgentBehavior.PROPERTY_NAME);
var ownerState = actor.GetProperty<OwnerState>(OwnerState.PROPERTY_NAME);
var companionState = actor.GetProperty<CompanionState>(CompanionState.PROPERTY_NAME);
var resetScriptId = companionState.DefaultBehaviorTreeId;
System.Console.WriteLine("DefaultBehaviorTreeId: " + resetScriptId);

ownerState.CanBeCaptured = true;
ownerState.OwnerId = string.Empty;

actor.SetProperty(
    OwnerState.PROPERTY_NAME,
    ownerState
);
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