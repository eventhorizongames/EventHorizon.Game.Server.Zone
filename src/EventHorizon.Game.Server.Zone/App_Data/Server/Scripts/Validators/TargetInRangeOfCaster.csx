/// <summary>
/// Id: target_in_range_of_caster
/// 
/// Caster - 
/// Target - 
/// Data: { min: number; max: number; }
/// Services: { Vector3: Vector3; }
/// </summary>

using System.Collections.Generic;
using System.Numerics;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Combat.Plugin.Skill.Model;

var caster = Data.Get<IObjectEntity>("Caster");
var target = Data.Get<IObjectEntity>("Target");
var validatorData = Data.Get<IDictionary<string, object>>("ValidatorData");

var min = (long)validatorData["min"];
var max = (long)validatorData["max"];

var distance = Vector3.Distance(
    caster.Transform.Position,
    target.Transform.Position
);

if (distance >= min && distance <= max)
{
    return new SkillValidatorResponse
    {
        Success = true
    };
}

return new SkillValidatorResponse
{
    Success = false,
    ErrorCode = "target_not_in_range",
    ErrorMessageTemplateKey = "targetNotInRange"
};