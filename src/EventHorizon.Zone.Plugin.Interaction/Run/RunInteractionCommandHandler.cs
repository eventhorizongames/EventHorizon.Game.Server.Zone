using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.Entity.Find;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.Core.Model.Player;
using EventHorizon.Zone.System.Combat.Events.Client.Messsage;
using EventHorizon.Zone.Plugin.Interaction.Events;
using EventHorizon.Zone.Plugin.Interaction.Model;
using EventHorizon.Zone.Plugin.Interaction.Script.Run;
using MediatR;
using EventHorizon.Zone.System.Combat.Model.Client.Messsage;

namespace EventHorizon.Zone.Plugin.Interaction.Run
{
    public struct RunInteractionCommandHandler : IRequestHandler<RunInteractionCommand>
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
            if (!IsValidInteraction(
                interactionEntity,
                ref player
            ))
            {
                await PublishMessageToPlayer(
                    player.ConnectionId,
                    "interaction_not_valid",
                    "Interaction or Player entity was not found."
                );
                return Unit.Value;
            }
            var interactionState = interactionEntity.GetProperty<InteractionState>(
                InteractionState.PROPERTY_NAME
            );
            if (!IsActiveInteraction(
                interactionState
            ))
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

        private static bool IsValidInteraction(
            IObjectEntity interactionEntity,
            ref PlayerEntity player
        )
        {
            return interactionEntity.IsFound()
                    && player.IsFound();
        }

        private static bool IsActiveInteraction(
            InteractionState interactionState
        )
        {
            return interactionState.Active
                    && interactionState.List != null;
        }

        private async Task PublishMessageToPlayer(
            string connectionId,
            string messageCode,
            string message
        )
        {
            await _mediator.Publish(
                new SingleClientActionMessageFromCombatSystemEvent
                {
                    ConnectionId = connectionId,
                    Data = new MessageFromCombatSystemData
                    {
                        MessageCode = messageCode,
                        Message = message
                    }
                }
            );
        }
    }
}