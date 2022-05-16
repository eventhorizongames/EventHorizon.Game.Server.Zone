using System.Threading.Tasks;
using Collections = System.Collections.Generic;
using EntityRegister = EventHorizon.Zone.Core.Events.Entity.Register;
using Logging = Microsoft.Extensions.Logging;
using ServerScriptsModel = EventHorizon.Zone.System.Server.Scripts.Model;

public class __SCRIPT__
    : ServerScriptsModel.ServerScript,
      EntityRegister.EntityUnRegisterEventObserver
{
    public string Id => "__SCRIPT__";
    public Collections.IEnumerable<string> Tags => new Collections.List<string> { "testing-tag" };

    private ServerScriptsModel.ServerScriptServices _services;
    private Logging.ILogger _logger;

    public async Task<ServerScriptResponse> Run(
        ServerScriptsModel.ServerScriptServices services,
        ServerScriptsModel.ServerScriptData data
    )
    {
        _services = services;
        _logger = services.Logger<__SCRIPT__>();
        _logger.LogDebug("__SCRIPT__ - Server Script");

        _logger.LogInformation($"I am here: Player: ");

        return new ServerScriptsModel.StandardServerScriptResponse(true, "observer_setup");
    }

    public Task Handle(EntityRegister.EntityUnRegisteredEvent args)
    {
        _logger.LogDebug("Testing");
        InMemoryGameState.Instance.RemovePlayer(_services, args.EntityId);
        return Task.CompletedTask;
    }
}
