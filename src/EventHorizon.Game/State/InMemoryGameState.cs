namespace EventHorizon.Game.State
{
    using System.Collections.Generic;

    public class InMemoryGameState
        : GameState
    {
        private readonly object _lock = new();

        public CurrentGameState CurrentGameState { get; private set; } = new CurrentGameState(new List<GamePlayerScore>());

        public void IncrementPlayer(
            long playerEntityId
        )
        {
            lock (_lock)
            {
                CurrentGameState = CurrentGameState.IncrementPlayer(
                    playerEntityId
                );
            }
        }

        public void RemovePlayer(
            long playerEntityId
        )
        {
            lock (_lock)
            {
                CurrentGameState = CurrentGameState.RemovePlayer(
                    playerEntityId
                );
            }
        }
    }
}
