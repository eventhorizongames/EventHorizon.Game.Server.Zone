namespace EventHorizon.Game.Server.Zone.Game.Query
{
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Game.Server.Zone.Game.State;
    using MediatR;

    public class QueryForCurrentGameStateHandler 
        : IRequestHandler<QueryForCurrentGameState, CurrentGameState>
    {
        private readonly GameState _gameState;

        public QueryForCurrentGameStateHandler(
            GameState gameState
        )
        {
            _gameState = gameState;
        }

        public Task<CurrentGameState> Handle(
            QueryForCurrentGameState request,
            CancellationToken cancellationToken
        )
        {
            return _gameState.CurrentGameState
                .FromResult();
        }
    }
}