using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Skill.Runner
{
    public struct RunSkillWithTargetOfEntityEvent : INotification
    {
        public string SkillId { get; set; }
        public long CasterId { get; set; }
        public long TargetId { get; set; }
    }
}