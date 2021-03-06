/// <summary>
/// Id: target_not_caster
/// var tagList = new string[] { "Type:SkillValidatorScript" };
/// 
/// Caster: { Id: string; } 
/// Target: { Id: string; }
/// Data: { }
/// </summary>

using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Combat.Plugin.Skill.Model;


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

        var caster = data.Get<IObjectEntity>("Caster");
        var target = data.Get<IObjectEntity>("Target");

        if (target.Id != caster.Id)
        {
            return new SkillValidatorResponse
            {
                Success = true
            };
        }

        return new SkillValidatorResponse
        {
            Success = false,
            ErrorCode = "caster_can_not_be_target",
            ErrorMessageTemplateKey = "casterCanNotTargetSelf"
        };
    }
}
