using EventHorizon.Zone.Core.Model.Player;
using MediatR;

namespace EventHorizon.Zone.System.Interaction.Events
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