using System.Threading.Tasks;

public class __SCRIPT__ : IStartupClientScript
{
    public string Id => "__SCRIPT__";

    public async Task Run(ScriptServices services, ScriptData data)
    {
        var logger = services.Logger<__SCRIPT__>();
        logger.LogDebug("__SCRIPT__ - Initialize Script");

        var observer = new __SCRIPT__Observer(services, data);
        data.Set("observer", observer);
        services.RegisterObserver(observer);
    }
}

public class __SCRIPT__Observer : Game_ClientActions_ClientActionGameStateUpdatedEventObserver
{
    private ILogger _logger;
    private readonly ScriptServices _scriptServices;
    private readonly ScriptData _scriptData;

    public __SCRIPT__Observer(ScriptServices services, ScriptData data)
    {
        _scriptServices = services;
        _scriptData = data;
        _logger = services.Logger<__SCRIPT__Observer>();
    }

    public Task Handle(Game_ClientActions_ClientActionGameStateUpdatedEvent notification)
    {
        // Set the GameState from the Server in this Client's ServerGameState
        ServerGameState.Set(notification.GameState);
        return Task.CompletedTask;
    }
}
