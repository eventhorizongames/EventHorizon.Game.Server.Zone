/// <summary>
/// Name: Behavior_Owner_ResetToDefaultBehavior.csx
/// 
/// Will update the Actor with no owner.
/// Will update behavior tree for the current actor to the CompanionState's DefaultBehaviorTreeId
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


using System.Collections.Generic;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Server.Scripts.Model;
using Microsoft.Extensions.Logging;

public class __SCRIPT__
    : ServerScript
{
    public string Id => "__SCRIPT__";
    public IEnumerable<string> Tags => new List<string> { "testing-tag" };

    public async Task<ServerScriptResponse> Run(
        ServerScriptServices services,
        ServerScriptData data
    )
    {
        var logger = services.Logger<__SCRIPT__>();
        logger.LogDebug("__SCRIPT__ - Server Script");

        var actor = data.Get<AgentEntity>("Actor");
        var agentBehaviorState = actor.GetProperty<AgentBehavior>(AgentBehavior.PROPERTY_NAME);
        var ownerState = actor.GetProperty<OwnerState>(OwnerState.PROPERTY_NAME);
        var companionState = actor.GetProperty<CompanionState>(CompanionState.PROPERTY_NAME);
        var resetScriptId = companionState.DefaultBehaviorTreeId;

        ownerState.CanBeCaptured = true;
        ownerState.OwnerId = string.Empty;

        actor.SetProperty(
            OwnerState.PROPERTY_NAME,
            ownerState
        );
        await services.Mediator.Send(
            new AgentUpdateEntityCommand(
                actor,
                AgentAction.SCRIPT
            )
        );

        await services.Mediator.Send(
            new ChangeActorBehaviorTreeCommand(
                actor,
                resetScriptId
            )
        );

        return new BehaviorScriptResponse(
            BehaviorNodeStatus.SUCCESS
        );
    }
}
