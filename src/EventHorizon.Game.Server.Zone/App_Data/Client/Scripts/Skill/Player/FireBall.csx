/*
data:
    entity: IObjectEntity

const skillId = "Skills_FireBall.json";
const entity = $data;
const noSelectedTargetMessage = $services.i18n["noTargetSelected"];

$utils.runClientScript(
    "skill.companion_targeted_skill",
    "Skill_Runners_RunSelectedTargetedSkill.js",
    {
        entity,
        skillId,
        noSelectedTargetMessage,
    }
);
*/

using System.Collections.Generic;
using System.Threading.Tasks;
using EventHorizon.Game.Client.Engine.Scripting.Api;
using EventHorizon.Game.Client.Engine.Scripting.Data;
using EventHorizon.Game.Client.Engine.Scripting.Services;
using EventHorizon.Game.Client.Engine.Systems.Entity.Api;
using EventHorizon.Game.Client.Engine.Systems.Scripting.Run;
using EventHorizon.Game.Server.ClientAction.Agent;
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

        var skillId = "Skills_FireBall.json";
        var entity = data.Get<IObjectEntity>("entity");
        var noTargetSelectedMessage = services.Localizer["noTargetSelected"];

        await services.Mediator.Send(
            new RunClientScriptCommand(
                "Skill_Runners_RunSelectedTargetedSkill",
                "skill.run_selected_targeted_skill",
                new Dictionary<string, object>
                {
                    { "entity", entity },
                    { "skillId", skillId },
                    { "noSelectedTargetMessage", noTargetSelectedMessage },
                }
            )
        );
    }
}
