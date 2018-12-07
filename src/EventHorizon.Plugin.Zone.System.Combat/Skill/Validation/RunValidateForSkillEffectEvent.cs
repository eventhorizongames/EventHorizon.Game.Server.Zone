
using System.Collections.Generic;
using EventHorizon.Game.Server.Zone.Model.Entity;
using EventHorizon.Plugin.Zone.System.Combat.Skill.Model;
using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Skill.Validation
{
    public struct RunValidateForSkillEffectEvent : IRequest<IEnumerable<SkillValidatorResponse>>
    {
        public SkillInstance Skill { get; set; }
        public SkillEffect SkillEffect { get; set; }
        public IObjectEntity Caster { get; set; }
        public IObjectEntity Target { get; set; }
    }
}