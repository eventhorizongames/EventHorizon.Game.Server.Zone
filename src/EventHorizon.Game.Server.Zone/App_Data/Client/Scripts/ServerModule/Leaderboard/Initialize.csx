using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GuiDispose = EventHorizon.Game.Client.Engine.Gui.Dispose;
using InputApi = EventHorizon.Game.Client.Engine.Input.Api;
using InputRegister = EventHorizon.Game.Client.Engine.Input.Register;
using ScriptingApi = EventHorizon.Game.Client.Engine.Scripting.Api;
using ScriptingData = EventHorizon.Game.Client.Engine.Scripting.Data;
using ScriptingServices = EventHorizon.Game.Client.Engine.Scripting.Services;

public class __SCRIPT__ : ScriptingApi.IClientScript
{
    public string Id => "__SCRIPT__";

    public async Task Run(ScriptingServices.ScriptServices services, ScriptingData.ScriptData data)
    {
        var logger = services.Logger<__SCRIPT__>();
        logger.LogDebug("Leader Board - Initialize Script");

        var layoutId = "GUI_LeaderBoard.json";
        var guiId = layoutId;

        var observer = new __SCRIPT__Observer(services, data, layoutId, guiId);

        data.Set("observer", observer);

        services.RegisterObserver(observer);

        await observer.OnChange();

        await observer.SetupKeyboardShortcut();
    }
}

public class __SCRIPT__Observer
    : Game_ClientActions_ClientActionGameStateUpdatedEventObserver,
      GuiLayoutDataChangedEventObserver
{
    private readonly ScriptingServices.ScriptServices _services;
    private readonly ScriptingData.ScriptData _scriptData;
    private readonly string _layoutId;
    private readonly string _guiId;

    private readonly string _entryLayoutId = "GUI_LeaderBoard-Entry.json";

    private bool _isOpen = false;

    public __SCRIPT__Observer(
        ScriptingServices.ScriptServices services,
        ScriptingData.ScriptData data,
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
            new InputRegister.RegisterInputCommand(
                new InputApi.InputOptions("t", HandleOpenOfLeaderBoard, HandleOfCloseLeaderBoard)
            )
        );
        if (result.Success)
        {
            _scriptData.Set("inputHandle", result.Result);
        }
    }

    private async Task HandleOpenOfLeaderBoard(InputApi.InputKeyEvent inputEvent)
    {
        if (_isOpen)
        {
            return;
        }
        _isOpen = true;
        await _services.Mediator.Send(new ShowGuiCommand(_guiId));
    }

    private async Task HandleOfCloseLeaderBoard(InputApi.InputKeyEvent inputEvent)
    {
        if (!_isOpen)
        {
            return;
        }
        _isOpen = false;
        await _services.Mediator.Send(new HideGuiCommand(_guiId));
    }

    public async Task Handle(GuiLayoutDataChangedEvent args)
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

    public Task Handle(Game_ClientActions_ClientActionGameStateUpdatedEvent _)
    {
        return OnEntryChange();
    }

    public async Task OnChange()
    {
        if (_scriptData.Get<bool>("active"))
        {
            await _services.Mediator.Send(new GuiDispose.DisposeOfGuiCommand(_guiId));
            _scriptData.Set("active", false);
        }
        var result = await _services.Mediator.Send(
            new CreateGuiCommand(_guiId, _layoutId, new List<IGuiControlData>())
        );

        if (result.Success)
        {
            await _services.Mediator.Send(new ActivateGuiCommand(_guiId));
            _scriptData.Set("active", true);
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
        //var gameState = await _services.Mediator.Send(new QueryForCurrentGameState());
        var gameState = ServerGameState.GameState;

        var playerList = gameState.Scores;

        foreach (var playerScore in playerList)
        {
            var guiLeaderboardId = $"GUI_LeaderBoard-Entry_{playerScore.PlayerEntityId}";
            var player = await GetPlayer(playerScore.PlayerEntityId.ToString());
            var score = playerScore.Score;
            // Remove Currently existing GUI
            await _services.Mediator.Send(new DisposeOfGuiCommand(guiLeaderboardId));

            var result = await _services.Mediator.Send(
                new CreateGuiCommand(
                    guiLeaderboardId,
                    _entryLayoutId,
                    parentControlId: await _services.Mediator.Send(
                        new GetGeneratedGuiControlId(_guiId, "leaderboard-panel")
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

            await _services.Mediator.Send(new ActivateGuiCommand(guiLeaderboardId));
        }
    }

    private async Task<IObjectEntity> GetPlayer(string playerEntityId)
    {
        return (
            await _services.Mediator.Send(
                new QueryForEntity(TagBuilder.CreateIdTag(playerEntityId))
            )
        ).Result.FirstOrDefault();
    }
}
