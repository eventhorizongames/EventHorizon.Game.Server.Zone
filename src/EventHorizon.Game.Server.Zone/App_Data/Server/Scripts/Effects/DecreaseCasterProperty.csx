/// <summary>
/// Effect Id: decrease_caster_property
/// </summary>
/// <returns></returns>
/// 
using System.Collections.Generic;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Combat.Events.Life;
using EventHorizon.Zone.System.Combat.Plugin.Skill.ClientAction;
using EventHorizon.Zone.System.Combat.Plugin.Skill.Model;

var caster = Data.Get<IObjectEntity>("Caster");
var target = Data.Get<IObjectEntity>("Target");
var effectData = Data.Get<IDictionary<string, object>>("EffectData");
var priorState = Data.Get<IDictionary<string, object>>("PriorState");

var propertyName = (string)effectData["propertyName"];
var valueProperty = (string)effectData["valueProperty"];
// TODO: Use the modifiers to change value amount.
var modifierPropertyName = (string)effectData["modifierPropertyName"];
var modifierValueProperty = (string)effectData["modifierValueProperty"];
var modifierBase = (long)effectData["modifierBase"];

var casterPropertyValue = caster.GetProperty<dynamic>(propertyName)[valueProperty];
var modiferPropertyValue = caster.GetProperty<dynamic>(modifierPropertyName)[modifierValueProperty];


var decreaseLifeEvent = new DecreaseLifePropertyEvent
{
    EntityId = caster.Id,
    Property = valueProperty,
    Points = modifierBase
};
await Services.Mediator.Publish(decreaseLifeEvent);

var actionData = new
{
    Id = caster.Id,
    PropertyName = propertyName.LowercaseFirstChar(),
    ValueProperty = valueProperty.LowercaseFirstChar(),
    Amount = modifierBase
};
var action = new ClientSkillActionEvent
{
    Action = "Actions_DecreaseProperty.js",
    Data = actionData
};

return SkillEffectScriptResponse
    .New()
    .Add(
        action
    );