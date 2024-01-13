namespace EventHorizon.Zone.System.Combat.Plugin.Skill.Events.Runner;

using global::System;
using global::System.Collections.Generic;
using global::System.Numerics;

using EventHorizon.Zone.Core.Model.Player;
using EventHorizon.Zone.System.Player.Plugin.Action.Model;

public struct RunSkillWithTargetOfEntityEvent : PlayerActionEvent
{
    public string ConnectionId { get; set; }
    public string SkillId { get; set; }
    public long CasterId { get; set; }
    public long TargetId { get; set; }
    public Vector3 TargetPosition { get; set; }

    public PlayerEntity Player { get; set; }

    public IDictionary<string, object> Data { get; set; }

    public PlayerActionEvent SetPlayer(
        PlayerEntity player
    )
    {
        ConnectionId = player.ConnectionId;
        Player = player;
        return this;
    }

    public PlayerActionEvent SetData(
        IDictionary<string, object> data
    )
    {
        SkillId = data.GetValueOrDefault(
            "skillId",
            ""
        );
        CasterId = data.GetValueOrDefault(
            "casterId",
            Player.Id
        );
        TargetId = data.GetValueOrDefault(
            "targetId",
            -1L
        );
        TargetPosition = data.GetValueOrDefault(
            "targetPosition",
            Vector3.Zero
        );
        Data = data;
        return this;
    }
}
