/// <summary>
/// Id: target_not_caster
/// 
/// Caster: { Id: string; } 
/// Target: { Id: string; }
/// Data: { }
/// </summary>

using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Combat.Skill.Model;

var caster = Data.Get<IObjectEntity>("Caster");
var target = Data.Get<IObjectEntity>("Target");

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