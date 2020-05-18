namespace EventHorizon.Game.State
{
    using System.Collections.ObjectModel;
    using System.Linq;

    public interface GameState
    {
        CurrentGameState CurrentGameState { get; }
        void RemovePlayer(
            long playerEntityId
        );
        void IncrementPlayer(
            long playerEntityId
        );
    }
}