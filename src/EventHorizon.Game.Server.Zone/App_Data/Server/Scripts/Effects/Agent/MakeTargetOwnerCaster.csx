/// <summary>
/// Effect Id: make_target_owner_castervar 
/// tagList = new string[] { "Type:SkillEffectScript" };
/// 
/// Caster: <see cref="EventHorizon.Zone.Core.Model.Entity.IObjectEntity" />  
/// Target: <see cref="EventHorizon.Zone.Core.Model.Entity.IObjectEntity" /> 
/// Data: { mesageTemplateKey: string; }
/// </summary>

using System.Collections.Generic;
using EventHorizon.Zone.Core.Events.Entity.Update;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Agent.Events.Update;
using EventHorizon.Zone.System.Agent.Model;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Change;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;
using EventHorizon.Zone.System.Agent.Plugin.Companion.Model;
using EventHorizon.Game.Increment;
using EventHorizon.Game.Model;
using EventHorizon.Zone.System.Combat.Plugin.Skill.ClientAction;
using EventHorizon.Zone.System.Combat.Plugin.Skill.Model;

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

        var caster = data.Get<IObjectEntity>("Caster");
        var target = data.Get<IObjectEntity>("Target");

        var casterGlobalId = caster.GlobalId;

        var casterCompanionManagement = caster.GetProperty<CompanionManagementState>(CompanionManagementState.PROPERTY_NAME);
        var targetOwner = target.GetProperty<OwnerState>(OwnerState.PROPERTY_NAME);

        targetOwner.OwnerId = casterGlobalId;
        targetOwner.CanBeCaptured = false;

        target.SetProperty(
            OwnerState.PROPERTY_NAME,
            targetOwner
        );

        // Update Target Properties
        await services.Mediator.Send(
            new UpdateEntityCommand(
                AgentAction.PROPERTY_CHANGED,
                target
            )
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
        caster.SetProperty(
            GamePlayerCaptureState.PROPERTY_NAME,
            playerCaptureState
        );
        await services.Mediator.Send(
            new UpdateEntityCommand(
                EntityAction.PROPERTY_CHANGED,
                caster
            )
        );

        var wasChanged = await services.Mediator.Send(
            new ChangeActorBehaviorTreeCommand(
                target,
                casterCompanionManagement.CapturedBehaviorTreeId
            )
        );

        await services.Mediator.Send(
            new IncrementPlayerScore(
                caster.Id
            )
        );

        var action = new ClientSkillActionEvent
        {
            Action = "Actions_Agent_AgentCaptured.js"
        };

        return SkillEffectScriptResponse
            .New()
            .Add(
                action
            );
    }
}
