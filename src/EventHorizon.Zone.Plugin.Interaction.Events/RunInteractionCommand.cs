using EventHorizon.Game.Server.Zone.Model.Player;
using MediatR;

namespace EventHorizon.Zone.Plugin.Interaction.Events
{
    public struct RunInteractionCommand : IRequest
    {
        public long InteractionEntityId { get; }
        public PlayerEntity Player { get; }

        public RunInteractionCommand(
            long interactionEntityId,
            PlayerEntity player
        )
        {
            InteractionEntityId = interactionEntityId;
            Player = player;
        }
    }
}