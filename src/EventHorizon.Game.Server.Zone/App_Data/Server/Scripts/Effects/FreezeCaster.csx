///
/// tagList = new string[] { "Type:SkillEffectScript" };
///
using System.Collections.Generic;
using EventHorizon.Zone.Core.Events.Entity.Movement;
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

// Stop the movement of the Caster Entity on the Server
await services.Mediator.Publish(
    new StopEntityMovementEvent
    {
        EntityId = caster.Id
    }
);

// Create Client action.
var freezeActionData = new
{
    Id = caster.Id
};
var freezeAction = new ClientSkillActionEvent
{
    Action = "Actions_FreezeEntity.js",
    Data = freezeActionData
};

return SkillEffectScriptResponse
    .New()
    .Add(
        freezeAction
    );
    }
}
