/// <summary>
/// Id: target_is_able_to_be_captured
/// 
/// Caster: ObjectEntity 
/// Target: ObjectEntity
/// TargetPosition: Vector3
/// PriorState: { }
/// Data: { }
/// </summary>


var canBeCaptured = Target.GetProperty<dynamic>("ownerState")["canBeCaptured"];

if (!canBeCaptured)
{
    return new SkillValidatorResponse
    {
        Success = false,
        ErrorCode = "target_is_not_able_to_be_captured",
        ErrorMessageTemplateKey = "targetIsNotAbleToBeCaptured",
    };
}
return new SkillValidatorResponse
{
    Success = true
};