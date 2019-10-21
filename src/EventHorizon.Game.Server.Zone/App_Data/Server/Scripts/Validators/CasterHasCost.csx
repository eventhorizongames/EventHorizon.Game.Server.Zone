/// <summary>
/// Id: caster_has_cost
/// 
/// Caster - 
/// Target - 
/// Data: { propertyName: string; valueProperty: string; cost: number; }
/// </summary>

using System.Collections.Generic;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Combat.Skill.Model;

var caster = Data.Get<IObjectEntity>("Caster");
var validatorData = Data.Get<IDictionary<string, object>>("ValidatorData");

var propertyName = (string)validatorData["propertyName"];
var valueProperty = (string)validatorData["valueProperty"];
var cost = (long)validatorData["cost"];

var casterPropertyValue = caster.GetProperty<dynamic>(propertyName)[valueProperty];

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
    ErrorMessageTemplateKey = "casterNotEnoughPoints",
    ErrorMessageTemplateData = new
    {
        Cost = cost,
        ValueProperty = valueProperty
    }
};