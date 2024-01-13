namespace EventHorizon.Zone.System.Combat.Plugin.Skill.Runner.Effect;

using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Combat.Plugin.Skill.Model;

using global::System.Collections.Generic;
using global::System.Numerics;

using MediatR;

public struct RunSkillEffectWithTargetOfEntityEvent : INotification
{
    public string ConnectionId { get; set; }
    public SkillEffect SkillEffect { get; set; }
    public IObjectEntity Caster { get; set; }
    public IObjectEntity Target { get; set; }
    public SkillInstance Skill { get; set; }
    public Vector3 TargetPosition { get; set; }
    public IDictionary<string, object> State { get; set; }

    public RunSkillEffectWithTargetOfEntityEvent(
        string connectionId,
        SkillEffect skillEffect,
        IObjectEntity caster,
        IObjectEntity target,
        SkillInstance skill,
        Vector3 targetPosition,
        IDictionary<string, object> state
    )
    {
        ConnectionId = connectionId;
        SkillEffect = skillEffect;
        Caster = caster;
        Target = target;
        Skill = skill;
        TargetPosition = targetPosition;
        State = state;
    }
}
