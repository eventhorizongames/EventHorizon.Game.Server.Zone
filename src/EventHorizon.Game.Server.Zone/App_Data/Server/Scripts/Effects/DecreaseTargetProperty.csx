/// <summary>
/// Effect Id: decrease_target_property
/// </summary>
/// <returns></returns>

using System.Collections.Generic;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Combat.Events.Life;
using EventHorizon.Zone.System.Combat.Skill.ClientAction;
using EventHorizon.Zone.System.Combat.Skill.Model;

var caster = Data.Get<IObjectEntity>("Caster");
var target = Data.Get<IObjectEntity>("Target");
var effectData = Data.Get<IDictionary<string, object>>("EffectData");

var propertyName = (string)effectData["propertyName"];
var valueProperty = (string)effectData["valueProperty"];
// TODO: Use the modifiers to change value amount.
var modifierPropertyName = (string)effectData["modifierPropertyName"];
var modifierValueProperty = (string)effectData["modifierValueProperty"];
var modifierBase = (long)effectData["modifierBase"];

var casterPropertyValue = caster.GetProperty<dynamic>(propertyName)[valueProperty];
var modiferPropertyValue = caster.GetProperty<dynamic>(modifierPropertyName)[modifierValueProperty];


await Services.Mediator.Publish(
    new DecreaseLifePropertyEvent
    {
        EntityId = target.Id,
        Property = valueProperty,
        Points = modifierBase
    }
);

var actionData = new
{
    Id = target.Id,
    PropertyName = propertyName.LowercaseFirstChar(),
    ValueProperty = valueProperty.LowercaseFirstChar(),
    Amount = modifierBase
};
var action = new ClientSkillActionEvent
{
    Action = "Actions_DecreaseProperty.js",
    Data = actionData
};

var messageActionData = new
{

    Amount = modifierBase
};
var messageAction = new ClientSkillActionEvent
{
    Action = "Actions_MesssageClient.js",
    Data = actionData
};

return new SkillEffectScriptResponse
{
    State = new Dictionary<string, object>
    {
        { "Damage", modifierBase }
    },
    ActionList = new List<ClientSkillActionEvent>
    {
        action
    }
};