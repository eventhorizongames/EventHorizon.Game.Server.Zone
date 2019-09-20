using System.Collections.Generic;
using System.Numerics;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Plugin.Zone.System.Combat.Skill.Model;
using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Skill.Runner.EffectRunner
{
    public struct RunSkillEffectWithTargetOfEntityEvent : INotification
    {
        public SkillEffect SkillEffect { get; internal set; }
        public string ConnectionId { get; set; }
        public IObjectEntity Caster { get; internal set; }
        public IObjectEntity Target { get; internal set; }
        public SkillInstance Skill { get; internal set; }
        public Vector3 TargetPosition { get; set; }
        public IDictionary<string, object> State { get; internal set; }
    }
}