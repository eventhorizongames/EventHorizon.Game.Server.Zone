using System.Threading.Tasks;
using Collections = System.Collections.Generic;
using EntityDataEvents = EventHorizon.Zone.Core.Events.Entity.Data;
using EntityModel = EventHorizon.Zone.Core.Model.Entity;
using Logging = Microsoft.Extensions.Logging;
using ServerScriptsModel = EventHorizon.Zone.System.Server.Scripts.Model;

public class __SCRIPT__
    : ServerScriptsModel.ServerScript,
      EntityDataEvents.PopulateEntityDataEventObserver
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

    public async Task Handle(EntityDataEvents.PopulateEntityDataEvent args)
    {
        var entity = args.Entity;

        if (entity.Type != EntityModel.EntityType.PLAYER)
        {
            return;
        }

        // Populate the Game State on the Player.
        entity.SetProperty(GamePlayerCaptureState.PROPERTY_NAME, GamePlayerCaptureState.New());
    }
}
