/*
data:
    active: bool
    observer: ObserverBase
    companionsCaught: IList<string>
    currentDisplayedCaught: IList<string>
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventHorizon.Game.Client.Engine.Entity.Tag;
using EventHorizon.Game.Client.Engine.Entity.Tracking.Query;
using EventHorizon.Game.Client.Engine.Gui.Activate;
using EventHorizon.Game.Client.Engine.Gui.Api;
using EventHorizon.Game.Client.Engine.Gui.Changed;
using EventHorizon.Game.Client.Engine.Gui.Create;
using EventHorizon.Game.Client.Engine.Gui.Dispose;
using EventHorizon.Game.Client.Engine.Gui.Generate;
using EventHorizon.Game.Client.Engine.Gui.Model;
using EventHorizon.Game.Client.Engine.Scripting.Api;
using EventHorizon.Game.Client.Engine.Scripting.Data;
using EventHorizon.Game.Client.Engine.Scripting.Services;
using EventHorizon.Game.Client.Systems.Player.Changed;
using EventHorizon.Game.Client.Systems.Player.Query;
using EventHorizon.Game.Server.ServerModule.Game.Model;
using Microsoft.Extensions.Logging;

public class __SCRIPT__
    : IClientScript
{
    public string Id => "__SCRIPT__";

    public async Task Run(
        ScriptServices services,
        ScriptData data
    )
    {
        var logger = services.Logger<__SCRIPT__>();
        logger.LogDebug("PlayerCaptures - Initialize Script");

        var layoutId = "GUI_PlayerCaptures.json";
        var guiId = layoutId;
        var observer = new __SCRIPT__Observer(
            services,
            data,
            layoutId,
            guiId
        );

        data.Set(
            "observer",
            observer
        );

        services.RegisterObserver(
            observer
        );

        await observer.OnChange();
    }
}

public class __SCRIPT__Observer
    : GuiLayoutDataChangedEventObserver,
    PlayerDetailsChangedEventObserver
{
    private readonly ScriptServices _services;
    private readonly ScriptData _data;
    private readonly string _layoutId;
    private readonly string _guiId;

    private readonly string _messageLayoutId = "GUI_PlayerCaptures-Message.json";

    public __SCRIPT__Observer(
        ScriptServices services,
        ScriptData data,
        string layoutId,
        string guiId
    )
    {
        _services = services;
        _data = data;
        _layoutId = layoutId;
        _guiId = guiId;

        _data.Set(
            "caughtCount",
            0
        );
        _data.Set(
            "companionsCaught",
            new List<string>()
        );
        _data.Set(
            "currentDisplayedCaught",
            new List<string>()
        );
    }

    public async Task Handle(
        GuiLayoutDataChangedEvent args
    )
    {
        if (args.Id == _layoutId)
        {
            await OnChange();
        }
        else if (args.Id == _messageLayoutId)
        {
            await BuildMessagesLayout();
        }
    }

    public async Task Handle(
        PlayerDetailsChangedEvent args
    )
    {
        var caughtCount = _data.Get<int>(
            "caughtCount"
        );
        // Get Current Player of Client
        var playerOption = await _services.Mediator.Send(
            new QueryForCurrentPlayer()
        );
        var player = playerOption.Result;
        // Validate Entity Details are for this Player
        if (!playerOption.Success)
        {
            return;
        }
        // Get GamePlayerCaptureState State from Player
        var captureState = player.GetProperty<IGamePlayerCaptureState>(
            IGamePlayerCaptureState.NAME
        );
        if (captureState == null)
        {
            return;
        }
        // Check GamePlayerCaptureState.CompanionsCaught
        if (captureState.CompanionsCaught.Count == 0
            || captureState.CompanionsCaught.Count < caughtCount
        )
        {
            // Reset GUI if zero or less than currently caughtCount
            await OnChange();
            return;
        }
        // Update data used in creation of UX
        _data.Set(
            "companionsCaught",
            captureState.CompanionsCaught
        );
        // Check for Main GUI is active
        if (!_data.Get<bool>("active"))
        {
            return;
        }

        await BuildMessagesLayout();
    }

    public async Task OnChange()
    {
        if (_data.Get<bool>("active"))
        {
            await _services.Mediator.Send(
                new DisposeOfGuiCommand(
                    _guiId
                )
            );
            _data.Set(
                "active",
                false
            );
        }
        var result = await _services.Mediator.Send(
            new CreateGuiCommand(
                _guiId,
                _layoutId,
                new List<IGuiControlData>()
            )
        );

        if (result.Success)
        {
            await _services.Mediator.Send(
                new ActivateGuiCommand(
                    _guiId
                )
            );
            _data.Set(
                "active",
                true
            );
            await BuildMessagesLayout();
        }
    }

    private async Task BuildMessagesLayout()
    {
        var caughtCount = _data.Get<int>(
            "caughtCount"
        );
        var companionsCaught = _data.Get<IList<string>>(
            "companionsCaught"
        );
        var currentDisplayedCaught = _data.Get<IList<string>>(
            "currentDisplayedCaught"
        );
        var caughtCompanionNameList = new List<string>();
        foreach (var globalId in companionsCaught.Where(globalId => !currentDisplayedCaught.Contains(globalId)))
        {
            var entityLookupResult = await _services.Mediator.Send(
                new QueryForEntity(
                    TagBuilder.CreateGlobalIdTag(
                        globalId
                    )
                )
            );
            if (entityLookupResult.Success
                && entityLookupResult.Result.Count() > 0)
            {
                caughtCompanionNameList.Add(
                    entityLookupResult.Result.FirstOrDefault().Name
                );
            }
        }
        // Get
        _data.Set(
            "currentDisplayedCaught",
            companionsCaught
        );
        foreach (var caughtCompanionName in caughtCompanionNameList)
        {
            caughtCount++;
            var newGuid = $"GUI_PlayerCaptures-Message_{caughtCount}";
            var parentControlId = await _services.Mediator.Send(
                new GetGeneratedGuiControlId(
                    newGuid,
                    "player_capture-panel"
                )
            );
            await _services.Mediator.Send(
                new CreateGuiCommand(
                    newGuid,
                    _messageLayoutId,
                    parentControlId: parentControlId,
                    controlDataList: new List<IGuiControlData>
                    {
                        new GuiControlDataModel
                        {
                            ControlId = "player_capture-message-immortal-name",
                            Options = new GuiControlOptionsModel
                            {
                                { 
                                    "text", 
                                    _services.Localize(
                                        "game:captureImmortalName",
                                        caughtCompanionName
                                    )
                                }
                            }
                        }
                    }
                )
            );

            await _services.Mediator.Send(
                new ActivateGuiCommand(
                    newGuid
                )
            );
        }
    }
}
