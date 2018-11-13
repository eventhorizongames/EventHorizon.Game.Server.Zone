using EventHorizon.Plugin.Zone.System.Combat.Skill.Model;
using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Skill.Find
{
    public struct FindSkillByIdEvent : IRequest<SkillInstance>
    {
        public string SkillId { get; set; }
    }
}