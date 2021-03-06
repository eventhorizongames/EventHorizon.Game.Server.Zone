/*
data:
    skillId: string
    entity: IObjectEntity
    noSelectedTargetMessage: string
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
        var message = data.Get<string>("noSelectedTargetMessage");

        var selectedTrackerModule = entity.GetModule<SelectedTrackerModule>(
            SelectedTrackerModule.MODULE_NAME
        );

        if (selectedTrackerModule.HasSelectedEntity)
        {
            await services.Mediator.Send(
                new RunClientScriptCommand(
                    "Skill_Runners_RunPlayerSkill",
                    "skill.run_player_skill",
                    new Dictionary<string, object>
                    {
                        { "casterId", entity.EntityId },
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
