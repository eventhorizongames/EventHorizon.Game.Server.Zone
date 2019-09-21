using System.Numerics;
using MediatR;

namespace EventHorizon.Zone.System.Combat.Events.Skill.Runner
{
    public struct RunSkillWithTargetOfEntityEvent : INotification
    {
        public string ConnectionId { get; set; }
        public string SkillId { get; set; }
        public long CasterId { get; set; }
        public long TargetId { get; set; }
        public Vector3 TargetPosition { get; set; }
    }
}