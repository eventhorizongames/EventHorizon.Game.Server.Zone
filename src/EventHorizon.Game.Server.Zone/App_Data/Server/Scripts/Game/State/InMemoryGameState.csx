using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ServerScriptsModel = EventHorizon.Zone.System.Server.Scripts.Model;

public class InMemoryGameState
{
    public static InMemoryGameState Instance { get; } = new InMemoryGameState();

    private readonly object _lock = new();

    public CurrentGameState CurrentGameState { get; private set; } =
        new CurrentGameState(new List<GamePlayerScore>());

    public void IncrementPlayer(
        ServerScriptsModel.ServerScriptServices services,
        long playerEntityId
    )
    {
        lock (_lock)
        {
            CurrentGameState = CurrentGameState.IncrementPlayer(playerEntityId);
            services.DataStore.AddOrUpdate("CurrentGameState", CurrentGameState);
        }
    }

    public void RemovePlayer(ServerScriptsModel.ServerScriptServices services, long playerEntityId)
    {
        lock (_lock)
        {
            CurrentGameState = CurrentGameState.RemovePlayer(playerEntityId);
            services.DataStore.AddOrUpdate("CurrentGameState", CurrentGameState);
        }
    }
}

public struct CurrentGameState
{
    public IList<GamePlayerScore> Scores { get; }

    public CurrentGameState(IList<GamePlayerScore> scores)
    {
        Scores = new ReadOnlyCollection<GamePlayerScore>(
            scores.OrderBy(a => a.Score).Reverse().ToList()
        );
    }

    public CurrentGameState IncrementPlayer(long playerEntityId)
    {
        var player = Scores.FirstOrDefault(player => player.PlayerEntityId == playerEntityId);
        var newScores = Scores.ToList();
        newScores.Remove(player);
        if (player.IsFound())
        {
            player = new GamePlayerScore(playerEntityId, 0);
        }
        newScores.Add(player.Increment());

        return new CurrentGameState(newScores);
    }

    public CurrentGameState RemovePlayer(long playerEntityId)
    {
        var player = Scores.FirstOrDefault(player => player.PlayerEntityId == playerEntityId);
        var newScores = Scores.ToList();
        newScores.Remove(player);

        return new CurrentGameState(newScores);
    }
}

public struct GamePlayerScore
{
    private readonly string _isFound;
    public long PlayerEntityId { get; }
    public int Score { get; }

    public GamePlayerScore(long playerEntityId, int score)
    {
        _isFound = "is_found";
        PlayerEntityId = playerEntityId;
        Score = score;
    }

    public GamePlayerScore Increment()
    {
        return new GamePlayerScore(PlayerEntityId, Score + 1);
    }

    public bool IsFound()
    {
        return string.IsNullOrEmpty(_isFound);
    }
}
