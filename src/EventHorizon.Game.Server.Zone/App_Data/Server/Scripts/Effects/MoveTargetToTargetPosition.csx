/// <summary>
/// Effect Id: move_target_to_target_position
/// tagList = new string[] { "Type:SkillEffectScript" };
/// 
/// Caster: ObjectEntity 
/// Target: ObjectEntity
/// TargetPosition: Vector3
/// PriorState: { }
/// Data: { }
/// Services: { Mediator: IMediator; I18n: I18nLookup; }
/// </summary>

using System.Collections.Generic;
using System.Numerics;
using EventHorizon.Zone.Core.Events.Entity.Movement;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Agent.Plugin.Move.Events;
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
var targetPosition = data.Get<Vector3>("TargetPosition");
var effectData = data.Get<IDictionary<string, object>>("EffectData");

await services.Mediator.Send(
    new MoveAgentToPositionEvent(
        target.Id,
        targetPosition
    )
);

return SkillEffectScriptResponse
    .New();
    }
}
