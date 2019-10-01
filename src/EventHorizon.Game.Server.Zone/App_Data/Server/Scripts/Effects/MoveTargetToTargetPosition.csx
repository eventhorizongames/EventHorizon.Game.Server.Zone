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

using EventHorizon.Zone.System.Agent.Plugin.Move.Events;

await Services.Mediator.Send(
    new MoveAgentToPositionEvent
    {
        AgentId = Target.Id,
        ToPosition = TargetPosition
    }
);

return new SkillEffectScriptResponse
{
    ActionList = new List<ClientSkillActionEvent>()
};