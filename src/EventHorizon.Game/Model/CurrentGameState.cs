namespace EventHorizon.Game.State
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public struct CurrentGameState
    {
        public IList<GamePlayerScore> Scores { get; }

        public CurrentGameState(
            IList<GamePlayerScore> scores
        )
        {
            Scores = new ReadOnlyCollection<GamePlayerScore>(
                scores.OrderBy(
                    a => a.Score
                ).Reverse().ToList()
            );
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
}
