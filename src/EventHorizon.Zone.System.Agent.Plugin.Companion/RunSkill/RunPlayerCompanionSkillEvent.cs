using System.Numerics;
using MediatR;

namespace EventHorizon.Zone.System.Agent.Plugin.Companion.RunSkill
{
    public struct RunPlayerCompanionSkillEvent : INotification
    {
        public string ConnectionId { get; }
        public long PlayerId { get; }
        public long CasterId { get; }
        public string SkillId { get; }
        public long TargetId { get; }
        public Vector3 TargetPosition { get; }

        public RunPlayerCompanionSkillEvent(
            string connectionId,
            long playerId,
            long casterId,
            string skillId,
            long targetId,
            Vector3 targetPosition
        )
        {
            this.ConnectionId = connectionId;
            this.PlayerId = playerId;
            this.CasterId = casterId;
            this.SkillId = skillId;
            this.TargetId = targetId;
            this.TargetPosition = targetPosition;
        }
    }
}