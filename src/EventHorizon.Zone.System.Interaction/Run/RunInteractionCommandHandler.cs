namespace EventHorizon.Zone.System.Interaction.Run
{
    using EventHorizon.Zone.Core.Events.Entity.Find;
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.System.Combat.Events.Client.Messsage;
    using EventHorizon.Zone.System.Combat.Model.Client.Messsage;
    using EventHorizon.Zone.System.Interaction.Events;
    using EventHorizon.Zone.System.Interaction.Model;
    using EventHorizon.Zone.System.Interaction.Script.Run;

    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    public class RunInteractionCommandHandler : IRequestHandler<RunInteractionCommand>
    {
        readonly IMediator _mediator;
        public RunInteractionCommandHandler(
            IMediator mediator
        )
        {
            _mediator = mediator;
        }
        public async Task<Unit> Handle(
            RunInteractionCommand request,
            CancellationToken cancellationToken
        )
        {
            var interactionEntity = await _mediator.Send(
                new GetEntityByIdEvent
                {
                    EntityId = request.InteractionEntityId
                }
            );
            var player = request.Player;
            if (!player.IsFound())
            {
                // If player is not found ignore the request.
                return Unit.Value;
            }
            if (!interactionEntity.IsFound())
            {
                await PublishMessageToPlayer(
                    player.ConnectionId,
                    "interaction_not_valid",
                    "Interaction entity was not found."
                );
                return Unit.Value;
            }
            var interactionState = interactionEntity.GetProperty<InteractionState>(
                InteractionState.PROPERTY_NAME
            );

            if (!interactionState.Active
                || interactionState.List.IsNull()
            )
            {
                await PublishMessageToPlayer(
                    player.ConnectionId,
                    "interaction_not_active",
                    "Interaction was not active."
                );
                return Unit.Value;
            }

            // Finally we can script interaction
            foreach (var interactionItem in interactionState.List)
            {
                await _mediator.Send(
                    new RunInteractionScriptCommand(
                        interactionItem,
                        interactionEntity,
                        player
                    )
                );
            }
            return Unit.Value;
        }

        private async Task PublishMessageToPlayer(
            string connectionId,
            string messageCode,
            string message
        )
        {
            await _mediator.Publish(
                SingleClientActionMessageFromCombatSystemEvent.Create(
                    connectionId,
                    new MessageFromCombatSystemData
                    {
                        MessageCode = messageCode,
                        Message = message
                    }
                )
            );
        }
    }
}
