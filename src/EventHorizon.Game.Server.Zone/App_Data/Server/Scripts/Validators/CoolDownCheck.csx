/// <summary>
/// Id - cool_down_check
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

var caster = Data.Get<IObjectEntity>("Caster");
var skill = Data.Get<SkillInstance>("Skill");

var skillState = caster.GetProperty<SkillState>("skillState");

var skillReady = Services.DateTime.Now > skillState
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