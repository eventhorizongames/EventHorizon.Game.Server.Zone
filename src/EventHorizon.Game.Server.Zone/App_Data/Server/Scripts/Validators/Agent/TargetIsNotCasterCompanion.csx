/// <summary>
/// Id: target_is_not_caster_companion
/// 
/// Caster: ObjectEntity 
/// Target: ObjectEntity
/// TargetPosition: Vector3
/// PriorState: { }
/// Data: { }
/// </summary>

using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Combat.Skill.Model;

var caster = Data.Get<IObjectEntity>("Caster");
var target = Data.Get<IObjectEntity>("Target");

var targetOwnerId = target.GetProperty<dynamic>("ownerState")["ownerId"];

if (targetOwnerId == caster.GlobalId)
{
    return new SkillValidatorResponse
    {
        Success = false,
        ErrorCode = "target_is_casters_companion",
        ErrorMessageTemplateKey = "targetIsCastersCompanion",
    };
}
return new SkillValidatorResponse
{
    Success = true
};