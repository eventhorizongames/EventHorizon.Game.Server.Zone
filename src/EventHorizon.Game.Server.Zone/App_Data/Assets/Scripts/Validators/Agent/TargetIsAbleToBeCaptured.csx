/// <summary>
/// Id: target_is_able_to_be_captured
/// 
/// Caster: ObjectEntity 
/// Target: ObjectEntity
/// TargetPosition: Vector3
/// PriorState: { }
/// Data: { }
/// </summary>


var canNotBeCaptured = Target.GetProperty<dynamic>("ownerState")["canNotBeCaptured"];

if (canNotBeCaptured)
{
    return new SkillValidatorResponse
    {
        Success = false,
        ErrorCode = "target_is_not_able_to_be_captured",
        ErrorMessageTemplateKey = "TargetIsNotAbleToBeCaptured",
    };
}
return new SkillValidatorResponse
{
    Success = true
};