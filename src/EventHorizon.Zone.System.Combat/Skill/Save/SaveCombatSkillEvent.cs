using EventHorizon.Zone.System.Combat.Skill.Model;
using MediatR;

namespace EventHorizon.Zone.System.Combat.Skill.Save
{
    public struct SaveCombatSkillEvent : IRequest<SkillInstance>
    {
        public SkillInstance Skill { get; set; }
    }
}