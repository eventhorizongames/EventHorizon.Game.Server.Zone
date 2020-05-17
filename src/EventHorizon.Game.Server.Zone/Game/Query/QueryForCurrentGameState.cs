namespace EventHorizon.Game.Server.Zone.Game.Query
{
    using EventHorizon.Game.Server.Zone.Game.State;
    using MediatR;

    public struct QueryForCurrentGameState : IRequest<CurrentGameState>
    {

    }
}