/// <summary>
/// Effect Id: make_target_owner_castervar 
/// tagList = new string[] { "Type:SkillEffectScript" };
/// 
/// Caster: <see cref="EventHorizon.Zone.Core.Model.Entity.IObjectEntity" />  
/// Target: <see cref="EventHorizon.Zone.Core.Model.Entity.IObjectEntity" /> 
/// Data: { mesageTemplateKey: string; }
/// </summary>

using System.Collections.Generic;
using System.Threading.Tasks;
using AgentModel = EventHorizon.Zone.System.Agent.Model;
using AgentPluginCompanionModel = EventHorizon.Zone.System.Agent.Plugin.Companion.Model;
using BehaviorChange = EventHorizon.Zone.System.Agent.Plugin.Behavior.Change;
using CombatPluginSkillModel = EventHorizon.Zone.System.Combat.Plugin.Skill.Model;
using EntityModel = EventHorizon.Zone.Core.Model.Entity;
using EntityUpdate = EventHorizon.Zone.Core.Events.Entity.Update;
using ScriptsModel = EventHorizon.Zone.System.Server.Scripts.Model;
using SkillClientAction = EventHorizon.Zone.System.Combat.Plugin.Skill.ClientAction;

public class __SCRIPT__ : ScriptsModel.ServerScript
{
    public string Id => "__SCRIPT__";
    public IEnumerable<string> Tags => new List<string> { "testing-tag" };

    public async Task<ServerScriptResponse> Run(
        ScriptsModel.ServerScriptServices services,
        ScriptsModel.ServerScriptData data
    )
    {
        var logger = services.Logger<__SCRIPT__>();
        logger.LogDebug("__SCRIPT__ - Server Script");

        var caster = data.Get<EntityModel.IObjectEntity>("Caster");
        var target = data.Get<EntityModel.IObjectEntity>("Target");

        var casterGlobalId = caster.GlobalId;

        var casterCompanionManagement =
            caster.GetProperty<AgentPluginCompanionModel.CompanionManagementState>(
                AgentPluginCompanionModel.CompanionManagementState.PROPERTY_NAME
            );
        var targetOwner = target.GetProperty<AgentPluginCompanionModel.OwnerState>(
            AgentPluginCompanionModel.OwnerState.PROPERTY_NAME
        );

        targetOwner.OwnerId = casterGlobalId;
        targetOwner.CanBeCaptured = false;

        target.SetProperty(AgentPluginCompanionModel.OwnerState.PROPERTY_NAME, targetOwner);

        // Update Target Properties
        await services.Mediator.Send(
            new EntityUpdate.UpdateEntityCommand(AgentModel.AgentAction.PROPERTY_CHANGED, target)
        );

        // Update Caster/Player CompanionCount
        if (!caster.ContainsProperty(GamePlayerCaptureState.PROPERTY_NAME))
        {
            caster = caster.SetProperty(
                GamePlayerCaptureState.PROPERTY_NAME,
                new GamePlayerCaptureState()
            );
        }
        var playerCaptureState = caster.GetProperty<GamePlayerCaptureState>(
            GamePlayerCaptureState.PROPERTY_NAME
        );
        playerCaptureState.CompanionsCaught.Add(target.GlobalId);
        playerCaptureState.Captures = playerCaptureState.CompanionsCaught.Count;
        playerCaptureState.ShownTenSecondMessage = false;
        playerCaptureState.ShownFiveSecondMessage = false;
        // TODO: Move "30" to a GameSetting
        playerCaptureState.EscapeCaptureTime = services.DateTime.Now.AddSeconds(30);
        caster.SetProperty(GamePlayerCaptureState.PROPERTY_NAME, playerCaptureState);
        await services.Mediator.Send(
            new EntityUpdate.UpdateEntityCommand(EntityAction.PROPERTY_CHANGED, caster)
        );

        var wasChanged = await services.Mediator.Send(
            new BehaviorChange.ChangeActorBehaviorTreeCommand(
                target,
                casterCompanionManagement.CapturedBehaviorTreeId
            )
        );

        await services.ObserverBroker.Trigger(new Game_Increment_IncrementPlayerScore(caster.Id));

        var action = new SkillClientAction.ClientSkillActionEvent
        {
            Action = "Actions_Agent_AgentCaptured.js"
        };

        return CombatPluginSkillModel.SkillEffectScriptResponse.New().Add(action);
    }
}
