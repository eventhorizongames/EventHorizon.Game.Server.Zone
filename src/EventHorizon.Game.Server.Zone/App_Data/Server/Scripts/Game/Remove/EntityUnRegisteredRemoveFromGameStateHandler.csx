using System.Threading.Tasks;
using Collections = System.Collections.Generic;
using EntityRegisterEvents = EventHorizon.Zone.Core.Events.Entity.Register;
using Logging = Microsoft.Extensions.Logging;
using ServerScriptsModel = EventHorizon.Zone.System.Server.Scripts.Model;

public class __SCRIPT__
    : ServerScriptsModel.ServerScript,
      EntityRegisterEvents.EntityUnRegisterEventObserver
{
    public string Id => "__SCRIPT__";
    public Collections.IEnumerable<string> Tags => new Collections.List<string> { };

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

        return new ServerScriptsModel.StandardServerScriptResponse(true, "observer_setup");
    }

    public async Task Handle(EntityRegisterEvents.EntityUnRegisteredEvent args)
    {
        await _services.ObserverBroker.Trigger(new Game_Clear_ClearPlayerScore(args.EntityId));
    }
}
