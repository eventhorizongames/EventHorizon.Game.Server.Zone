namespace EventHorizon.Game.State
{
    using System.Collections.Generic;

    public class InMemoryGameState : GameState
    {
        public CurrentGameState CurrentGameState { get; private set; } = new CurrentGameState(new List<GamePlayerScore>());

        public void IncrementPlayer(
            long playerEntityId
        )
        {
            CurrentGameState = CurrentGameState.IncrementPlayer(
                playerEntityId
            );
        }

        public void RemovePlayer(
            long playerEntityId
        )
        {
            CurrentGameState = CurrentGameState.RemovePlayer(
                playerEntityId
            );
        }
    }
}