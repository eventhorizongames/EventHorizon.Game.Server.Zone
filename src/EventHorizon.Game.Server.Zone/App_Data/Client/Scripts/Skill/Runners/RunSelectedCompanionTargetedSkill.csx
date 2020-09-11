/*
data:
    entity: IObjectEntity
    skillId: string
    noSelectionsMessage: string

const entity = $data.entity;
const skillId = $data.skillId;
const message = $data.noSelectionsMessage;

const selectedCompanionTrackerModule = entity.getProperty(
    "SELECTED_COMPANION_TRACKER_MODULE_NAME"
);
const selectedTrackerModule = entity.getProperty(
    "SELECTED_TRACKER_MODULE_NAME"
);
if (
    selectedTrackerModule.hasSelectedEntity &&
    selectedCompanionTrackerModule.hasSelectedEntity
) {
    $utils.runClientScript(
        "skill.targeted_skill",
        "Skill_Runners_RunCompanionSkill.js", {
            casterId: selectedCompanionTrackerModule.selectedEntityId,
            targetId: selectedTrackerModule.selectedEntityId,
            skillId
        }
    );
} else {
    $services.eventService.publish(
        $utils.createEvent("MessageFromCombatSystem", {
            message
        })
    );
}
*/

using System.Collections.Generic;
using System.Threading.Tasks;
using EventHorizon.Game.Client.Engine.Scripting.Api;
using EventHorizon.Game.Client.Engine.Scripting.Data;
using EventHorizon.Game.Client.Engine.Scripting.Services;
using EventHorizon.Game.Client.Engine.Systems.Entity.Api;
using EventHorizon.Game.Client.Engine.Systems.Scripting.Run;
using EventHorizon.Game.Client.Systems.Player.Modules.SelectedCompanionTracker.Api;
using EventHorizon.Game.Client.Systems.Player.Modules.SelectedTracker.Api;
using EventHorizon.Game.Server.ServerModule.CombatSystemLog.ClientAction.Message;
using Microsoft.Extensions.Logging;

public class __SCRIPT__
    : IClientScript
{
    public string Id => "__SCRIPT__";

    public async Task Run(
        ScriptServices services,
        ScriptData data
    )
    {
        var logger = services.Logger<__SCRIPT__>();
        logger.LogDebug("__SCRIPT__ Script");

        var entity = data.Get<IObjectEntity>("entity");
        var skillId = data.Get<string>("skillId");
        var message = data.Get<string>("noSelectionsMessage");

        var selectedCompanionTrackerModule = entity.GetModule<SelectedCompanionTrackerModule>(
            SelectedCompanionTrackerModule.MODULE_NAME
        );

        var selectedTrackerModule = entity.GetModule<SelectedTrackerModule>(
            SelectedTrackerModule.MODULE_NAME
        );

        if (selectedTrackerModule.HasSelectedEntity
            && selectedCompanionTrackerModule.HasSelectedEntity)
        {
            await services.Mediator.Send(
                new RunClientScriptCommand(
                    "Skill_Runners_RunCompanionSkill",
                    "skill.run_companion_skill",
                    new Dictionary<string, object>
                    {
                        { "casterId", selectedCompanionTrackerModule.SelectedEntityId },
                        { "targetId", selectedTrackerModule.SelectedEntityId },
                        { "skillId", skillId },
                    }
                )
            );
        }
        else 
        {
            await services.Mediator.Publish(
                new ClientActionMessageFromCombatSystemEvent(
                    message
                )
            );
        }
    }
}
