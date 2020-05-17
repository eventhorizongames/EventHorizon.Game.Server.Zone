namespace EventHorizon.Game.Server.Zone.Game.State
{
    using System.Collections.Generic;
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

    public class InMemoryGameState : GameState
    {
        private CurrentGameState _currentGameState = new CurrentGameState(new List<GamePlayerScore>());

        public CurrentGameState CurrentGameState => _currentGameState;

        public void IncrementPlayer(
            long playerEntityId
        )
        {
            _currentGameState = _currentGameState.IncrementPlayer(
                playerEntityId
            );
        }

        public void RemovePlayer(
            long playerEntityId
        )
        {
            _currentGameState = _currentGameState.RemovePlayer(
                playerEntityId
            );
        }
    }

    public struct CurrentGameState
    {
        public IList<GamePlayerScore> Scores { get; }

        public CurrentGameState(
            IList<GamePlayerScore> scores
        )
        {
            Scores = new ReadOnlyCollection<GamePlayerScore>(scores);
        }

        public CurrentGameState IncrementPlayer(
            long playerEntityId
        )
        {
            var player = Scores.FirstOrDefault(
                player => player.PlayerEntityId == playerEntityId
            );
            var newScores = Scores.ToList();
            newScores.Remove(player);
            if (player.IsFound())
            {
                player = new GamePlayerScore(
                    playerEntityId,
                    0
                );
            }
            newScores.Add(
                player.Increment()
            );

            return new CurrentGameState(
                newScores
            );
        }

        public CurrentGameState RemovePlayer(
            long playerEntityId
        )
        {
            var player = Scores.FirstOrDefault(
                player => player.PlayerEntityId == playerEntityId
            );
            var newScores = Scores.ToList();
            newScores.Remove(player);

            return new CurrentGameState(
                newScores
            );
        }
    }

    public struct GamePlayerScore
    {
        private string _isFound;
        public long PlayerEntityId { get; }
        public int Score { get; }

        public GamePlayerScore(
            long playerEntityId,
            int score
        )
        {
            _isFound = "is_found";
            PlayerEntityId = playerEntityId;
            Score = score;
        }

        public GamePlayerScore Increment()
        {
            return new GamePlayerScore(
                PlayerEntityId,
                Score + 1
            );
        }

        public GamePlayerScore Clear()
        {
            return new GamePlayerScore(
                PlayerEntityId,
                0
            );
        }

        public bool IsFound()
        {
            return string.IsNullOrEmpty(_isFound);
        }
    }
}