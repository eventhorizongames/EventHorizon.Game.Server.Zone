namespace EventHorizon.Game.Server.Zone.Player.Action.Direction
{
    using System.Collections.Generic;

    using EventHorizon.Zone.Core.Model.Player;
    using EventHorizon.Zone.System.Player.Plugin.Action.Model;

    public struct MovePlayerEvent : PlayerActionEvent
    {
        public PlayerEntity Player { get; set; }
        public long MoveDirection { get; set; }
        public IDictionary<string, object> Data { get; private set; }

        public PlayerActionEvent SetPlayer(
            PlayerEntity player
        )
        {
            Player = player;
            return this;
        }

        public PlayerActionEvent SetData(
            IDictionary<string, object> data
        )
        {
            MoveDirection = data.GetValueOrDefault(
                "moveDirection",
                -1L
            );
            Data = data;
            return this;
        }
    }
}
