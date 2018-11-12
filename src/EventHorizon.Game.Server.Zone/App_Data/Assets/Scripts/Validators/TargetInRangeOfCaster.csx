/// <summary>
/// Caster - 
/// Target - 
/// Data: { min: number; max: number; }
/// Services: { Vector3: Vector3; }
/// </summary>
var min = (long)Data["min"];
var max = (long)Data["max"];

var distance = Vector3.Distance(
    Caster.Position.CurrentPosition,
    Target.Position.CurrentPosition
);

if (distance >= min && distance <= max) {
    return new SkillValidatorResponse
    {
        Success = true
    };
}

return new SkillValidatorResponse
{
    Success = false
};