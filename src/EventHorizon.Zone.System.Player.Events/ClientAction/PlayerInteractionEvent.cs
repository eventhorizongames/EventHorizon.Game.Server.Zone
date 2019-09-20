using EventHorizon.Zone.Core.Model.Player;
using MediatR;

namespace EventHorizon.Zone.System.Player.Events.ClientAction
{
    public struct PlayerInteractionEvent : INotification
    {
        // public string InteractionId { get; }
        public PlayerEntity Player { get; }
        public long InteractionEntityId { get; }

        public PlayerInteractionEvent(
            PlayerEntity player, 
            long interactionEntityId
        ) : this()
        {
            Player = player;
            InteractionEntityId = interactionEntityId;
        }
    }
}