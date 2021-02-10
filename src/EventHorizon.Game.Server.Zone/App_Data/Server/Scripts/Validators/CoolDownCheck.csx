/// <summary>
/// Id - cool_down_check
/// var tagList = new string[] { "Type:SkillValidatorScript" };
/// 
/// Caster - 
/// Target - 
/// Skill - { id: string; }
/// Data: { message: string; messageCode: string; }
/// Services: { Mediator: IMediator; DateTime: IDateTimeService; }
/// </summary>

using System.Linq;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Combat.Plugin.Skill.Model;
using EventHorizon.Zone.System.Combat.Plugin.Skill.Model.Entity;

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
        var skill = data.Get<SkillInstance>("Skill");

        var skillState = caster.GetProperty<SkillState>("skillState");

        var skillReady = services.DateTime.Now > skillState
                        .SkillMap.Get(
                            skill.Id
                        ).CooldownFinishes;

        if (skillReady)
        {
            return new SkillValidatorResponse
            {
                Success = true
            };
        }

        return new SkillValidatorResponse
        {
            Success = false,
            ErrorCode = "skill_not_ready",
            ErrorMessageTemplateKey = "skillNotReady",
        };
    }
}
