using EventHorizon.Zone.System.Combat.Plugin.Skill.Model;
using MediatR;

namespace EventHorizon.Zone.System.Combat.Plugin.Skill.Find
{
    public struct FindSkillByIdEvent : IRequest<SkillInstance>
    {
        public string SkillId { get; set; }
    }
} 