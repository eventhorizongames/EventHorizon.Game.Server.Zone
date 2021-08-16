namespace EventHorizon.Game.State
{
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

        public bool IsFound()
        {
            return string.IsNullOrEmpty(_isFound);
        }
    }
}
