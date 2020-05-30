using EventHorizon.Zone.System.Combat.Plugin.Skill.Model;
using MediatR;

namespace EventHorizon.Zone.System.Combat.Plugin.Skill.Save
{
    public struct SaveCombatSkillEvent : IRequest<SkillInstance>
    {
        public SkillInstance Skill { get; set; }
    }
}