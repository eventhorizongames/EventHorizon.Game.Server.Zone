namespace EventHorizon.Game.Increment
{
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Game.Client;
    using EventHorizon.Game.State;
    using MediatR;

    public class IncrementPlayerScoreHandler : IRequestHandler<IncrementPlayerScore>
    {
        private readonly IMediator _mediator;
        private readonly GameState _gameState;

        public IncrementPlayerScoreHandler(
            IMediator mediator, 
            GameState gameState
        )
        {
            _mediator = mediator;
            _gameState = gameState;
        }

        public async Task<Unit> Handle(
            IncrementPlayerScore request,
            CancellationToken cancellationToken
        )
        {
            // Increment Player Score on Game State
            _gameState.IncrementPlayer(
                request.PlayerEntityId
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

            return Unit.Value;
        }
    }
}