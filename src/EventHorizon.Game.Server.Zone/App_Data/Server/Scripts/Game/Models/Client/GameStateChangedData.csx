using ClientModel = EventHorizon.Zone.Core.Model.Client;

public struct GameStateChangedData : ClientModel.IClientActionData
{
    public CurrentGameState GameState { get; set; }
}
