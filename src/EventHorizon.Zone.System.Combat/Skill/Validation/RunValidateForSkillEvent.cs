using System.Collections.Generic;
using System.Numerics;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.Combat.Skill.Model;
using MediatR;

namespace EventHorizon.Zone.System.Combat.Skill.Validation
{
    public struct RunValidateForSkillEvent : IRequest<SkillValidatorResponse>
    {
        public SkillInstance Skill { get; set; }
        public IObjectEntity Caster { get; set; }
        public IObjectEntity Target { get; set; }
        public Vector3 TargetPosition { get; set; }
    }
}