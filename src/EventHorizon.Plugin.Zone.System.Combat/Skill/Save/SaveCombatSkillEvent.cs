using EventHorizon.Plugin.Zone.System.Combat.Skill.Model;
using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Skill.Save
{
    public struct SaveCombatSkillEvent : IRequest<SkillInstance>
    {
        public SkillInstance Skill { get; set; }
    }
}