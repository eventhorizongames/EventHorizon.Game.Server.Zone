using System.Collections.Generic;

using EventHorizon.Zone.Core.Model.Player;
using EventHorizon.Zone.System.Player.Plugin.Action.Model;

namespace EventHorizon.Game.Server.Zone.Player.Move.Stop
{
    public struct StopPlayerEvent : PlayerActionEvent
    {
        public PlayerEntity Player { get; private set; }

        public IDictionary<string, object> Data { get; set; }

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
            Data = data;
            return this;
        }
    }
}
