/*
data:
    observer: ObserverBase
*/

using System;
using System.Threading.Tasks;
using EventHorizon.Game.Client.Engine.Gui.Model;
using EventHorizon.Game.Client.Engine.Scripting.Api;
using EventHorizon.Game.Client.Engine.Scripting.Data;
using EventHorizon.Game.Client.Engine.Scripting.Services;
using EventHorizon.Game.Server.ServerModule.CombatSystemLog.ClientAction.Message;
using EventHorizon.Game.Server.ServerModule.SystemLog.Message;
using Microsoft.Extensions.Logging;

public class __SCRIPT__
    : IClientScript
{
    public string Id => "__SCRIPT__";

    public Task Run(
        ScriptServices services,
        ScriptData data
    )
    {
        var logger = services.Logger<__SCRIPT__>();
        logger.LogDebug("Combat System Log - Initialize Script");

        var observer = new __SCRIPT__Observer(
            services,
            data
        );
        data.Set(
            "observer",
            observer
        );
        services.RegisterObserver(
            observer
        );

        return Task.CompletedTask;
    }
}

public class __SCRIPT__Observer
    : ClientActionMessageFromCombatSystemEventObserver
{
    private readonly ScriptServices _scriptServices;
    private readonly ScriptData _scriptData;
    private readonly ILogger _logger;

    public __SCRIPT__Observer(
        ScriptServices services,
        ScriptData data
    )
    {
        _scriptServices = services;
        _scriptData = data;
        _logger = services.Logger<__SCRIPT__Observer>();
    }

    public async Task Handle(
        ClientActionMessageFromCombatSystemEvent args
    )
    {
        _logger.LogDebug(
            "Message Data: {Args}",
            args
        );
        if (string.IsNullOrWhiteSpace(
            args.Message
        ))
        {
            _logger.LogWarning(
                "No Message"
            );
            return;
        }
        await _scriptServices.Mediator.Publish(
            new ClientActionMessageFromSystemEvent(
                args.Message,
                new GuiControlOptionsModel
                {
                    { "color", "red" },
                    { 
                        "text", 
                        _scriptServices.Localize(
                            "combatSystem:system"
                        ) 
                    },
                },
                new GuiControlOptionsModel()
            )
        );
    }
}
