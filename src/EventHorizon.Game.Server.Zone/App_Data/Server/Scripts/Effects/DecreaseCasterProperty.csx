/// <summary>
/// Effect Id: decrease_caster_property
/// tagList = new string[] { "Type:SkillEffectScript" };
/// </summary>
/// <returns></returns>
/// 
using System.Collections.Generic;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Combat.Events.Life;
using EventHorizon.Zone.System.Combat.Plugin.Skill.ClientAction;
using EventHorizon.Zone.System.Combat.Plugin.Skill.Model;

    
using System.Collections.Generic;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Server.Scripts.Model;
using Microsoft.Extensions.Logging;

public class __SCRIPT__
    : ServerScript
{
    public string Id => "__SCRIPT__";
    public IEnumerable<string> Tags => new List<string> { "testing-tag" };

    public async Task<ServerScriptResponse> Run(
        ServerScriptServices services,
        ServerScriptData data
    )
    {
        var logger = services.Logger<__SCRIPT__>();
        logger.LogDebug("__SCRIPT__ - Server Script");

var caster = data.Get<IObjectEntity>("Caster");
var target = data.Get<IObjectEntity>("Target");
var effectData = data.Get<IDictionary<string, object>>("EffectData");
var priorState = data.Get<IDictionary<string, object>>("PriorState");

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
await services.Mediator.Publish(decreaseLifeEvent);

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
    }
}
