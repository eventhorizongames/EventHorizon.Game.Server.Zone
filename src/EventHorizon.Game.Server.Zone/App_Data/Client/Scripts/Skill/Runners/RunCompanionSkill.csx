/*
data:
    casterId: number
    targetId: number
    skillId: string

$services.eventService.publish(
    $utils.createEvent("Player.PLAYER_ACTION_EVENT", {
        action: "Player.RUN_SKILL_ON_COMPANION",
        data: $data
    })
);
 */

using System.Collections.Generic;
using System.Threading.Tasks;
using EventHorizon.Game.Client.Engine.Scripting.Api;
using EventHorizon.Game.Client.Engine.Scripting.Data;
using EventHorizon.Game.Client.Engine.Scripting.Services;
using EventHorizon.Game.Client.Engine.Systems.Entity.Api;
using EventHorizon.Game.Client.Engine.Systems.Scripting.Run;
using EventHorizon.Game.Client.Systems.Player.Action.Model;
using EventHorizon.Game.Client.Systems.Player.Action.Model.Send;
using EventHorizon.Game.Client.Systems.Player.Modules.SelectedCompanionTracker.Api;
using EventHorizon.Game.Client.Systems.Player.Modules.SelectedTracker.Api;
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

        var casterId = data.Get<long>("casterId");
        var targetId = data.Get<long>("targetId");
        var skillId = data.Get<string>("skillId");

        await services.Mediator.Publish(
            new InvokePlayerActionEvent(
                "Player.RUN_SKILL_ON_COMPANION",
                new PlayerActionDataDictionary
                {
                    { "casterId", casterId },
                    { "targetId", targetId },
                    { "skillId", skillId },
                }
            )
        );
    }
}
