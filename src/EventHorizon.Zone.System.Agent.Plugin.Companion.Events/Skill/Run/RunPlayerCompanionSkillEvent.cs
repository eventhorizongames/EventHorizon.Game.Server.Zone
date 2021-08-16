using System.Collections.Generic;
using System.Numerics;

using EventHorizon.Zone.Core.Model.Player;
using EventHorizon.Zone.System.Player.Plugin.Action.Model;

namespace EventHorizon.Zone.System.Agent.Plugin.Companion.Events.Skill.Run
{
    public struct RunPlayerCompanionSkillEvent : PlayerActionEvent
    {
        public string ConnectionId { get; private set; }
        public long PlayerId { get; private set; }
        public long CasterId { get; private set; }
        public string SkillId { get; private set; }
        public long TargetId { get; private set; }
        public Vector3 TargetPosition { get; private set; }

        public IDictionary<string, object> Data { get; private set; }

        public PlayerEntity Player { get; private set; }

        public PlayerActionEvent SetPlayer(
            PlayerEntity player
        )
        {
            ConnectionId = player.ConnectionId;
            Player = player;
            PlayerId = player.Id;
            return this;
        }

        public PlayerActionEvent SetData(
            IDictionary<string, object> data
        )
        {
            CasterId = data.GetValueOrDefault(
                "casterId",
                -1L
            );
            SkillId = data.GetValueOrDefault(
                "skillId",
                ""
            );
            TargetId = data.GetValueOrDefault(
                "targetId",
                -1L
            );
            TargetPosition = data.GetValueOrDefault(
                "targetPosition",
                new Vector3(9_999_999, 9_999_999, 9_999_999)
            );
            Data = data;
            return this;
        }
    }
}
