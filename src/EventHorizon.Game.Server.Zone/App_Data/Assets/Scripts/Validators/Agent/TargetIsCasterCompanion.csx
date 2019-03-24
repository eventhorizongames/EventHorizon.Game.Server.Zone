/// <summary>
/// Id: target_is_caster_companion
/// 
/// Caster: ObjectEntity 
/// Target: ObjectEntity
/// TargetPosition: Vector3
/// PriorState: { }
/// Data: { }
/// </summary>


var targetOwnerId = Target.GetProperty<dynamic>("ownerState")["ownerId"];

if (targetOwnerId == Caster.GlobalId)
{
    return new SkillValidatorResponse
    {
        Success = true
    };
}

return new SkillValidatorResponse
{
    Success = false,
    ErrorCode = "target_is_not_casters_companion",
    ErrorMessageTemplateKey = "targetIsNotCastersCompanion",
};