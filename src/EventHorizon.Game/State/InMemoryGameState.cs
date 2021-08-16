namespace EventHorizon.Game.State
{
    using System.Collections.Generic;

    public class InMemoryGameState : GameState
    {
        private object _LOCK = new object();
        public CurrentGameState CurrentGameState { get; private set; } = new CurrentGameState(new List<GamePlayerScore>());

        public void IncrementPlayer(
            long playerEntityId
        )
        {
            lock (_LOCK)
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
            lock (_LOCK)
            {
                CurrentGameState = CurrentGameState.RemovePlayer(
                    playerEntityId
                );
            }
        }
    }
}
