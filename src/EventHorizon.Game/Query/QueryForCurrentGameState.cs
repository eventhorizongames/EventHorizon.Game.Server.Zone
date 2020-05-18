namespace EventHorizon.Game.Query
{
    using EventHorizon.Game.State;
    using MediatR;

    public struct QueryForCurrentGameState : IRequest<CurrentGameState>
    {

    }
}