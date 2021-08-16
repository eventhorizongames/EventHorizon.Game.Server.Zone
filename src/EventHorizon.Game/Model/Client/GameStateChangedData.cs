namespace EventHorizon.Game.Model.Client
{
    using EventHorizon.Game.State;
    using EventHorizon.Zone.Core.Model.Client;

    public struct GameStateChangedData : IClientActionData
    {
        public CurrentGameState GameState { get; set; }
    }
}
