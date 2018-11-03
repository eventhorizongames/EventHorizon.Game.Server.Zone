using System.Numerics;
using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Skill.Runner
{
    public struct RunSkillWithTargetOfPositionEvent : INotification
    {
        public string SkillId { get; set; }
        public long CasterId { get; set; }
        public Vector3 Target { get; set; }
    }
}