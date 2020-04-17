/// <summary>
/// Effect Id: move_target_to_target_position
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
using EventHorizon.Zone.System.Combat.Skill.ClientAction;
using EventHorizon.Zone.System.Combat.Skill.Model;

var caster = Data.Get<IObjectEntity>("Caster");
var target = Data.Get<IObjectEntity>("Target");
var targetPosition = Data.Get<Vector3>("TargetPosition");
var effectData = Data.Get<IDictionary<string, object>>("EffectData");

await Services.Mediator.Send(
    new MoveAgentToPositionEvent(
        target.Id,
        targetPosition
    )
);

return new SkillEffectScriptResponse
{
    ActionList = new List<ClientSkillActionEvent>()
};