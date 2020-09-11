/*
data:
    entity: IObjectEntity

const skillId = "Skills_CaptureTarget.json";
const entity = $data;
const noSelectedTargetMessage = $services.i18n["noTargetToCaptureSelected"];

$utils.runClientScript(
    "skill.targeted_skill",
    "Skill_Runners_RunSelectedTargetedSkill.js",
    {
        entity,
        skillId,
        noSelectedTargetMessage,
    }
);

 */

/*
data:
   observer: ObserverBase
   active: bool
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
        
        var skillId = "Skills_CaptureTarget.json";
        var entity = data.Get<IObjectEntity>("entity");
        var noSelectedTargetMessage = services.Localizer["noSelectedTargetMessage"];

        await services.Mediator.Send(
            new RunClientScriptCommand(
                "Skill_Runners_RunSelectedTargetedSkill",
                "skill.targeted_skill",
                new Dictionary<string, object>
                {
                    { "entity", entity },
                    { "skillId", skillId },
                    { "noSelectedTargetMessage", noSelectedTargetMessage },
                }
            )
        );
    }
}
