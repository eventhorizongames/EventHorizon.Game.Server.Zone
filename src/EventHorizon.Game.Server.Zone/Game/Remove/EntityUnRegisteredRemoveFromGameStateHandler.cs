namespace EventHorizon.Game.Server.Zone.Game.Remove
{
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Game.Server.Zone.Game.Client;
    using EventHorizon.Game.Server.Zone.Game.State;
    using EventHorizon.Zone.Core.Events.Entity.Register;
    using MediatR;

    public class EntityUnRegisteredRemoveFromGameStateHandler : INotificationHandler<EntityUnRegisteredEvent>
    {
        private readonly IMediator _mediator;
        private readonly GameState _gameState;

        public EntityUnRegisteredRemoveFromGameStateHandler(
            IMediator mediator,
            GameState gameState
        )
        {
            _mediator = mediator;
            _gameState = gameState;
        }

        public async Task Handle(
            EntityUnRegisteredEvent notification,
            CancellationToken cancellationToken
        )
        {
            _gameState.RemovePlayer(
                notification.EntityId
            );
            
            // Publish Game State Changed Action to All Clients
            await _mediator.Publish(
                ClientActionGameStateChangedToAllEvent.Create(
                    new Model.Client.GameStateChangedData
                    {
                        GameState = _gameState.CurrentGameState,
                    }
                )
            );
        }
    }
}