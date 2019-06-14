/// <summary>
/// Effect Id: decrease_target_property
/// </summary>
/// <returns></returns>

var propertyName = (string)Data["propertyName"];
var valueProperty = (string)Data["valueProperty"];
// TODO: Use the modifiers to change value amount.
var modifierPropertyName = (string)Data["modifierPropertyName"];
var modifierValueProperty = (string)Data["modifierValueProperty"];
var modifierBase = (long)Data["modifierBase"];

var casterPropertyValue = Caster.GetProperty<dynamic>(propertyName)[valueProperty];
var modiferPropertyValue = Caster.GetProperty<dynamic>(modifierPropertyName)[modifierValueProperty];


var decreaseLifeEvent = new DecreaseLifePropertyEvent
{
    EntityId = Target.Id,
    Property = valueProperty,
    Points = modifierBase
};
Services.Mediator.Publish(decreaseLifeEvent);

var actionData = new
{
    Id = Target.Id,
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