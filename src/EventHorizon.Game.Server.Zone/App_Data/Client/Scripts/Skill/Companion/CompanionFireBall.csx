/*
data:
    entity: IObjectEntity

const entity = $data;
const skillId = "Skills_FireBall.json";
const noSelectionsMessage = $services.i18n["noSelectionsMessage"];

$utils.runClientScript(
    "skill.companion_targeted_skill",
    "Skill_Runners_RunSelectedCompanionTargetedSkill.js",
    {
        entity,
        skillId,
        noSelectionsMessage,
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
        var noSelectionsMessage = services.Localizer["noSelectionsMessage"];

        await services.Mediator.Send(
            new RunClientScriptCommand(
                "Skill_Runners_RunSelectedCompanionTargetedSkill",
                "skill.run_selected_companion_targeted_skill",
                new Dictionary<string, object>
                {
                    { "entity", entity },
                    { "skillId", skillId },
                    { "noSelectionsMessage", noSelectionsMessage },
                }
            )
        );
    }
}
