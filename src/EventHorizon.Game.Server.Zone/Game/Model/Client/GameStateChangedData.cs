namespace EventHorizon.Game.Server.Zone.Game.Model.Client
{
    using EventHorizon.Game.Server.Zone.Game.State;
    using EventHorizon.Zone.Core.Model.Client;

    public class GameStateChangedData : IClientActionData
    {
        public CurrentGameState GameState { get; set; }
    }
}