using EventHorizon.Game.Server.Zone.Model.Entity;
using EventHorizon.Plugin.Zone.System.Combat.Skill.Model;
using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Skill.Runner.EffectRunner
{
    public struct RunSkillEffectWithTargetOfEntityEvent : INotification
    {
        public SkillEffect SkillEffect { get; internal set; }
        public IObjectEntity Caster { get; internal set; }
        public IObjectEntity Target { get; internal set; }
    }
}