/*
data:
    active: bool
    observer: ObserverBase
    inputHandle: string
*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using EventHorizon.Game.Client.Engine.Gui.Dispose;
using EventHorizon.Game.Client.Engine.Scripting.Services;
using EventHorizon.Game.Client.Engine.Scripting.Api;
using EventHorizon.Game.Client.Engine.Scripting.Data;
using Microsoft.Extensions.Logging;
using EventHorizon.Game.Client.Engine.Gui.Create;
using EventHorizon.Game.Client.Engine.Gui.Api;
using EventHorizon.Game.Client.Engine.Gui.Model;
using EventHorizon.Game.Client.Engine.Gui.Activate;
using EventHorizon.Game.Client.Engine.Gui.Scripting.Observers;
using EventHorizon.Game.Server.ServerModule.CaptureMessaging.ClientAction.Show;
using EventHorizon.Game.Server.ServerModule.SystemLog.Message;
using EventHorizon.Game.Client.Engine.Gui.Update;
using EventHorizon.Game.Client.Core.Timer.Api;
using EventHorizon.Game.Client.Engine.Gui.Hide;
using EventHorizon.Game.Client.Engine.Gui.Show;
using EventHorizon.Game.Server.ServerModule.Game.Query;
using EventHorizon.Game.Client.Engine.Gui.Changed;
using System.Linq;
using EventHorizon.Game.Client.Engine.Entity.Tracking.Query;
using EventHorizon.Game.Client.Engine.Entity.Tag;
using EventHorizon.Game.Client.Engine.Systems.Entity.Api;
using EventHorizon.Game.Client.Engine.Gui.Generate;
using EventHorizon.Game.Server.ServerModule.Game.Updated;
using EventHorizon.Game.Client.Engine.Input.Register;
using EventHorizon.Game.Client.Engine.Input.Api;

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
        logger.LogInformation("Leader Board - Initialize Script");

        var layoutId = "GUI_LeaderBoard.json";
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

        await observer.SetupKeyboardShortcut();
    }
}

public class __SCRIPT__Observer
    : GameStateUpdatedEventObserver,
    GuiLayoutDataChangedEventObserver
{
    private readonly ScriptServices _services;
    private readonly ScriptData _scriptData;
    private readonly string _layoutId;
    private readonly string _guiId;

    private readonly string _entryLayoutId = "GUI_LeaderBoard-Entry.json";

    private bool _isOpen = false;

    public __SCRIPT__Observer(
        ScriptServices services,
        ScriptData data,
        string layoutId,
        string guiId
    )
    {
        _services = services;
        _scriptData = data;
        _layoutId = layoutId;
        _guiId = guiId;
    }

    public async Task SetupKeyboardShortcut()
    {
        var result = await _services.Mediator.Send(
            new RegisterInputCommand(
                new InputOptions(
                    "t",
                    HandleOpenOfLeaderBoard,
                    HandleOfCloseLeaderBoard
                )
            )
        );
        if (result.Success)
        {
            _scriptData.Set(
                "inputHandle",
                result.Result
            );
        }
    }

    private async Task HandleOpenOfLeaderBoard(
        InputKeyEvent inputEvent
    )
    {
        if (_isOpen)
        {
            return;
        }
        _isOpen = true;
        await _services.Mediator.Send(
            new ShowGuiCommand(
                _guiId
            )
        );
    }

    private async Task HandleOfCloseLeaderBoard(
        InputKeyEvent inputEvent
    )
    {
        if (!_isOpen)
        {
            return;
        }
        _isOpen = false;
        await _services.Mediator.Send(
            new ShowGuiCommand(
                _guiId
            )
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
        else if (args.Id == _entryLayoutId)
        {
            await OnEntryChange();
        }
    }

    public Task Handle(
        GameStateUpdatedEvent _
    )
    {
        return OnEntryChange();
    }

    public async Task OnChange()
    {
        if (_scriptData.Get<bool>("active"))
        {
            await _services.Mediator.Send(
                new DisposeOfGuiCommand(
                    _guiId
                )
            );
            _scriptData.Set(
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
            _scriptData.Set(
                "active",
                true
            );
            await BuildEntriesLayout();
        }
    }

    private async Task OnEntryChange()
    {
        await OnChange();
        await BuildEntriesLayout();
    }

    private async Task BuildEntriesLayout()
    {
        var gameState = await _services.Mediator.Send(
            new QueryForCurrentGameState()
        );

        if (!gameState.Success)
        {
            return;
        }

        var playerList = gameState.Result.Scores;

        foreach (var playerScore in playerList)
        {
            var guiLeaderboardId = $"GUI_LeaderBoard-Entry_{playerScore.PlayerEntityId}";
            var player = await GetPlayer(
                playerScore.PlayerEntityId.ToString()
            );
            var score = playerScore.Score;
            // Remove Currently existing GUI
            await _services.Mediator.Send(
                new DisposeOfGuiCommand(
                    guiLeaderboardId
                )
            );

            var result = await _services.Mediator.Send(
                new CreateGuiCommand(
                    guiLeaderboardId,
                    _entryLayoutId,
                    parentControlId: await _services.Mediator.Send(
                        new GetGeneratedGuiControlId(
                            _guiId,
                            "leaderboard-panel"
                        )
                    ),
                    controlDataList: new List<IGuiControlData>
                    {
                        new GuiControlDataModel
                        {
                            ControlId = "leaderboard-entry",
                            Options = new GuiControlOptionsModel
                            {
                                { "text", $"Player [{player.Name}]: {score}" }
                            }
                        }
                    }
                )
            );
            if (!result.Success)
            {
                // Filed to Create Gui, ignore probably no Gui template.
                return;
            }

            await _services.Mediator.Send(
                new ActivateGuiCommand(
                    guiLeaderboardId
                )
            );
        }
    }

    private async Task<IObjectEntity> GetPlayer(
        string playerEntityId
    )
    {
        return (await _services.Mediator.Send(
            new QueryForEntity(
                TagBuilder.CreateIdTag(
                    playerEntityId
                )
            )
        )).Result.FirstOrDefault();
    }
}
