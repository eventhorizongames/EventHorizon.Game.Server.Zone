namespace EventHorizon.Zone.System.Interaction.Events
{
    using EventHorizon.Zone.Core.Model.Player;

    using MediatR;

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
