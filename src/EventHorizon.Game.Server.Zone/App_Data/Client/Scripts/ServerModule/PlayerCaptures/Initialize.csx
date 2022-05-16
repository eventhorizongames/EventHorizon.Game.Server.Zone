using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntityQuery = EventHorizon.Game.Client.Engine.Entity.Tracking.Query;
using GuiActivate = EventHorizon.Game.Client.Engine.Gui.Activate;
using GuiApi = EventHorizon.Game.Client.Engine.Gui.Api;
using GuiChanged = EventHorizon.Game.Client.Engine.Gui.Changed;
using GuiCreate = EventHorizon.Game.Client.Engine.Gui.Create;
using GuiGenerate = EventHorizon.Game.Client.Engine.Gui.Generate;
using GuiModel = EventHorizon.Game.Client.Engine.Gui.Model;
using PlayerChanged = EventHorizon.Game.Client.Systems.Player.Changed;
using PlayerQuery = EventHorizon.Game.Client.Systems.Player.Query;
using ScriptingApi = EventHorizon.Game.Client.Engine.Scripting.Api;
using ScriptingData = EventHorizon.Game.Client.Engine.Scripting.Data;
using ScriptingServices = EventHorizon.Game.Client.Engine.Scripting.Services;

public class __SCRIPT__ : ScriptingApi.IClientScript
{
    public string Id => "__SCRIPT__";

    public async Task Run(ScriptingServices.ScriptServices services, ScriptingData.ScriptData data)
    {
        var logger = services.Logger<__SCRIPT__>();
        logger.LogDebug("PlayerCaptures - Initialize Script");

        var layoutId = "GUI_PlayerCaptures.json";
        var guiId = layoutId;
        var observer = new __SCRIPT__Observer(services, data, layoutId, guiId);

        data.Set("observer", observer);

        services.RegisterObserver(observer);

        await observer.OnChange();
    }
}

public class __SCRIPT__Observer
    : GuiChanged.GuiLayoutDataChangedEventObserver,
      PlayerChanged.PlayerDetailsChangedEventObserver
{
    private readonly ScriptingServices.ScriptServices _services;
    private readonly ScriptingData.ScriptData _data;
    private readonly string _layoutId;
    private readonly string _guiId;

    private readonly string _messageLayoutId = "GUI_PlayerCaptures-Message.json";

    public __SCRIPT__Observer(
        ScriptingServices.ScriptServices services,
        ScriptingData.ScriptData data,
        string layoutId,
        string guiId
    )
    {
        _services = services;
        _data = data;
        _layoutId = layoutId;
        _guiId = guiId;

        _data.Set("caughtCount", 0);
        _data.Set("companionsCaught", new List<string>());
        _data.Set("currentDisplayedCaught", new List<string>());
    }

    public async Task Handle(GuiChanged.GuiLayoutDataChangedEvent args)
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

    public async Task Handle(PlayerChanged.PlayerDetailsChangedEvent args)
    {
        var caughtCount = _data.Get<int>("caughtCount");
        // Get Current Player of Client
        var playerOption = await _services.Mediator.Send(new PlayerQuery.QueryForCurrentPlayer());
        var player = playerOption.Result;
        // Validate Entity Details are for this Player
        if (!playerOption.Success)
        {
            return;
        }
        // Get GamePlayerCaptureState State from Player
        var captureState = player.GetProperty<Game_State_GamePlayerCaptureState>(
            Game_State_GamePlayerCaptureState.NAME
        );
        if (captureState == null)
        {
            return;
        }
        // Check GamePlayerCaptureState.CompanionsCaught
        if (
            captureState.CompanionsCaught.Count == 0
            || captureState.CompanionsCaught.Count < caughtCount
        )
        {
            // Reset GUI if zero or less than currently caughtCount
            _data.Set("caughtCount", captureState.CompanionsCaught.Count);
            _data.Set("companionsCaught", new List<string>());
            _data.Set("currentDisplayedCaught", new List<string>());
            await OnChange();
            return;
        }
        // Update data used in creation of UX
        _data.Set("companionsCaught", captureState.CompanionsCaught);
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
            await _services.Mediator.Send(new GuiDispose.DisposeOfGuiCommand(_guiId));
            _data.Set("active", false);
        }
        var result = await _services.Mediator.Send(
            new GuiCreate.CreateGuiCommand(_guiId, _layoutId, new List<GuiApi.IGuiControlData>())
        );

        if (result.Success)
        {
            await _services.Mediator.Send(new GuiActivate.ActivateGuiCommand(_guiId));
            _data.Set("active", true);
            await BuildMessagesLayout();
        }
    }

    private async Task BuildMessagesLayout()
    {
        var caughtCount = _data.Get<int>("caughtCount");
        var companionsCaught = _data.Get<IList<string>>("companionsCaught");
        var currentDisplayedCaught = _data.Get<IList<string>>("currentDisplayedCaught");
        var caughtCompanionNameList = new List<string>();
        foreach (
            var globalId in companionsCaught.Where(
                globalId => !currentDisplayedCaught.Contains(globalId)
            )
        )
        {
            var entityLookupResult = await _services.Mediator.Send(
                new EntityQuery.QueryForEntity(TagBuilder.CreateGlobalIdTag(globalId))
            );
            if (entityLookupResult.Success && entityLookupResult.Result.Count() > 0)
            {
                caughtCompanionNameList.Add(entityLookupResult.Result.FirstOrDefault().Name);
            }
        }
        // Get
        _data.Set("currentDisplayedCaught", companionsCaught);
        foreach (var caughtCompanionName in caughtCompanionNameList)
        {
            caughtCount++;
            var newGuid = $"GUI_PlayerCaptures-Message_{caughtCount}";
            var parentControlId = await _services.Mediator.Send(
                new GuiGenerate.GetGeneratedGuiControlId(_guiId, "player_capture-panel")
            );
            await _services.Mediator.Send(
                new GuiCreate.CreateGuiCommand(
                    newGuid,
                    _messageLayoutId,
                    parentControlId: parentControlId,
                    controlDataList: new List<GuiApi.IGuiControlData>
                    {
                        new GuiControlDataModel
                        {
                            ControlId = "player_capture-message-immortal-name",
                            Options = new GuiModel.GuiControlOptionsModel
                            {
                                {
                                    "text",
                                    _services.Localizer[
                                        "game:captureImmortalName",
                                        caughtCompanionName
                                    ]
                                }
                            }
                        }
                    }
                )
            );

            await _services.Mediator.Send(new GuiActivate.ActivateGuiCommand(newGuid));
        }
        _data.Set("caughtCount", caughtCount);
    }
}
