using EventHorizon.Zone.System.Combat.Skill.Model;
using MediatR;

namespace EventHorizon.Zone.System.Combat.Skill.Find
{
    public struct FindSkillByIdEvent : IRequest<SkillInstance>
    {
        public string SkillId { get; set; }
    }
} 