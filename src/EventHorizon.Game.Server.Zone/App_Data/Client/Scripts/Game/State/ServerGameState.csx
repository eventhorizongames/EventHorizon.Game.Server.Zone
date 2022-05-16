public class ServerGameState
{
    public static GameState GameState { get; private set; } = new GameState();

    public static void Set(GameState gameState)
    {
        if (!gameState.IsNotNull())
        {
            return;
        }
        GameState = gameState;
    }
}
