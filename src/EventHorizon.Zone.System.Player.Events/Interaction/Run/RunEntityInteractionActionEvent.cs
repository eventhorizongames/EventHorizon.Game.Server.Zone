using System.Collections.Generic;

using EventHorizon.Zone.Core.Model.Player;
using EventHorizon.Zone.System.Player.Plugin.Action.Model;

namespace EventHorizon.Zone.System.Player.Events.Interaction.Run
{
    public struct RunEntityInteractionActionEvent : PlayerActionEvent
    {
        public PlayerEntity Player { get; private set; }
        public long InteractionEntityId { get; private set; }

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
            InteractionEntityId = data.GetValueOrDefault<long>(
                "interactionEntityId",
                -1
            );
            Data = data;
            return this;
        }
    }
}
