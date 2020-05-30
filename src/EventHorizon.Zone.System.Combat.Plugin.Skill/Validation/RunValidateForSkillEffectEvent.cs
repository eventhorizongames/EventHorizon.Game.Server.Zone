
using System.Collections.Generic;
using System.Numerics;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Combat.Plugin.Skill.Model;
using MediatR;

namespace EventHorizon.Zone.System.Combat.Plugin.Skill.Validation
{
    public struct RunValidateForSkillEffectEvent : IRequest<IEnumerable<SkillValidatorResponse>>
    {
        public SkillInstance Skill { get; set; }
        public SkillEffect SkillEffect { get; set; }
        public IObjectEntity Caster { get; set; }
        public IObjectEntity Target { get; set; }
        public Vector3 TargetPosition { get; set; }
    }
}