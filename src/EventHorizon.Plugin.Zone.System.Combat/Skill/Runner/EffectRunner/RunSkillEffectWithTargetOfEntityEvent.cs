using System.Collections.Generic;
using EventHorizon.Game.Server.Zone.Model.Entity;
using EventHorizon.Plugin.Zone.System.Combat.Skill.Model;
using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Skill.Runner.EffectRunner
{
    public struct RunSkillEffectWithTargetOfEntityEvent : INotification
    {
        public SkillEffect SkillEffect { get; internal set; }
        public IObjectEntity Caster { get; internal set; }
        public string ConnectionId { get; set; }
        public IObjectEntity Target { get; internal set; }
        public IDictionary<string, object> State { get; internal set; }
    }
}