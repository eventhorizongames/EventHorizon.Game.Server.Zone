/// <summary>
/// Id: caster_has_cost
/// 
/// Caster - 
/// Target - 
/// Data: { propertyName: string; valueProperty: string; cost: number }
/// </summary>

var propertyName = (string)Data["propertyName"];
var valueProperty = (string)Data["valueProperty"];
var cost = (long)Data["cost"];

var casterPropertyValue = Caster.GetProperty<dynamic>(propertyName)[valueProperty];

if (casterPropertyValue >= cost)
{
    return new SkillValidatorResponse
    {
        Success = true
    };
}

return new SkillValidatorResponse
{
    Success = false,
    ErrorCode = "caster_not_enough_points",
    ErrorMessageTemplateKey = "CasterNotEnoughPoints",
    ErrorMessageTemplateData = new 
    {
        Cost = cost,
        ValueProperty = valueProperty
    }
};