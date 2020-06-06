namespace EventHorizon.Game.Clear
{
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Game.Client;
    using EventHorizon.Game.State;
    using MediatR;

    public class ClearPlayerScoreHandler 
        : IRequestHandler<ClearPlayerScore>
    {
        private readonly IMediator _mediator;
        private readonly GameState _gameState;

        public ClearPlayerScoreHandler(
            IMediator mediator,
            GameState gameState
        )
        {
            _mediator = mediator;
            _gameState = gameState;
        }

        public async Task<Unit> Handle(
            ClearPlayerScore request, 
            CancellationToken cancellationToken
        )
        {
            _gameState.RemovePlayer(
                request.EntityId
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
