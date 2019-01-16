using System.Numerics;
using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Skill.Runner
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