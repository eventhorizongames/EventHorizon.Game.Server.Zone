/*
data:
    entity: IObjectEntity
    active: boolean
    guiId: string
    observer: ObserverBase
    updateObserver: ObserverBase
*/

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventHorizon.Game.Client.Core.Factory.Api;
using EventHorizon.Game.Client.Core.Timer.Api;
using EventHorizon.Game.Client.Engine.Gui.Activate;
using EventHorizon.Game.Client.Engine.Gui.Api;
using EventHorizon.Game.Client.Engine.Gui.Changed;
using EventHorizon.Game.Client.Engine.Gui.Create;
using EventHorizon.Game.Client.Engine.Gui.Dispose;
using EventHorizon.Game.Client.Engine.Gui.Hide;
using EventHorizon.Game.Client.Engine.Gui.Model;
using EventHorizon.Game.Client.Engine.Gui.Scripting.Observers;
using EventHorizon.Game.Client.Engine.Gui.Show;
using EventHorizon.Game.Client.Engine.Gui.Update;
using EventHorizon.Game.Client.Engine.Scripting.Api;
using EventHorizon.Game.Client.Engine.Scripting.Data;
using EventHorizon.Game.Client.Engine.Scripting.Services;
using EventHorizon.Game.Client.Engine.Systems.Entity.Api;
using EventHorizon.Game.Client.Engine.Systems.Entity.Model;
using EventHorizon.Game.Client.Systems.Entity.Changed;
using EventHorizon.Game.Client.Systems.Local.Modules.MeshManagement.Api;
using EventHorizon.Game.Client.Systems.Local.Modules.MeshManagement.Set;
using EventHorizon.Game.Server.Game.CaptureMessaging.ClientAction.Show;
using EventHorizon.Game.Server.ServerModule.SystemLog.Message;
using EventHorizon.Zone.Core.Events.Map.Cost;
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
        logger.LogDebug("__SCRIPT__ - Initialize Script");

        var entity = data.Get<IObjectEntity>("entity");

        var layoutId = "GUI_Module_HealthModule.json";
        var guiId = $"health_module-{entity.EntityId}";

        var observer = new ScriptGuiLayoutDataChangedObserver(
            services,
            data,
            layoutId,
            guiId,
            () =>
            {
                return new List<IGuiControlData>
                {
                    new GuiControlDataModel
                    {
                        ControlId = "gui_health_module-bar",
                        Options = new GuiControlOptionsModel
                        {
                            {
                                "__metadata",
                                new GuiControlOptionsModel.GuiControlMetadataOptionModel
                                {
                                    ModelOptions = new List<string>{ "textOptions" }
                                }
                            },
                            { "percent", __SCRIPT__Observer.GetEntityPercent(entity) },
                            {
                                "textOptions",
                                new GuiControlOptionsModel
                                {
                                    { "text", __SCRIPT__Observer.GetEntityText(entity) },
                                }
                            },
                        },
                        LinkWith = entity.GetModule<IMeshModule>(
                            IMeshModule.MODULE_NAME
                        ).Mesh
                    }
                };
            }
        );
        services.RegisterObserver(
            observer
        );
        await observer.OnChange();
        data.Set(
            ScriptGuiLayoutDataChangedObserver.DataKey(
                layoutId,
                guiId,
                "observer"
            ),
            observer
        );

        var updateObserver = new __SCRIPT__Observer(
            services,
            data,
            observer,
            layoutId,
            guiId,
            entity
        );
        services.RegisterObserver(
            updateObserver
        );
        // await updateObserver.Setup();

        data.Set(
            "updateObserver",
            updateObserver
        );
        data.Set(
            "guiId",
            guiId
        );
    }
}

public class __SCRIPT__Observer
    : MeshSetEventObserver,
    EntityChangedSuccessfullyEventObserver
{
    public static string GetEntityText(
        IObjectEntity entity
    )
    {
        return entity.Name ?? string.Empty;
    }
    public static int GetEntityPercent(
        IObjectEntity entity
    )
    {
        var lifeState = entity.GetProperty<StandardObjectProperty>("lifeState");

        if (lifeState.ContainsKey("healthPoints")
            && lifeState.ContainsKey("maxHealthPoints")
        )
        {
            var healthPoints = lifeState["healthPoints"].To<decimal>();
            var maxHealthPoints = lifeState["maxHealthPoints"].To<decimal>();

            var percent = (healthPoints / maxHealthPoints) * 100;
            return (int)percent;
        }

        return 100;
    }

    private readonly ScriptServices _services;
    private readonly ScriptData _data;
    private readonly ScriptGuiLayoutDataChangedObserver _rootObserver;
    private readonly string _layoutId;
    private readonly string _guiId;
    private readonly IObjectEntity _entity;

    public __SCRIPT__Observer(
        ScriptServices services,
        ScriptData data,
        ScriptGuiLayoutDataChangedObserver rootObserver,
        string layoutId,
        string guiId,
        IObjectEntity entity
    )
    {
        _services = services;
        _data = data;
        _rootObserver = rootObserver;
        _layoutId = layoutId;
        _guiId = guiId;
        _entity = entity;
    }

    public async Task Handle(
        EntityChangedSuccessfullyEvent args
    )
    {
        if (args.EntityId != _entity.EntityId)
        {
            return;
        }
        await _rootObserver.OnChange();
    }

    public async Task Handle(MeshSetEvent args)
    {
        if (args.ClientId != _entity.ClientId)
        {
            return;
        }

        await _rootObserver.OnChange();
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
                new List<IGuiControlData>
                {
                    new GuiControlDataModel
                    {
                        ControlId = "gui_health_module-bar",
                        Options = new GuiControlOptionsModel
                        {
                            {
                                "__metadata",
                                new GuiControlOptionsModel.GuiControlMetadataOptionModel
                                {
                                    ModelOptions = new List<string>{ "textOptions" }
                                }
                            },
                            { "percent", __SCRIPT__Observer.GetEntityPercent(_entity) },
                            {
                                "textOptions",
                                new GuiControlOptionsModel
                                {
                                    { "text", __SCRIPT__Observer.GetEntityText(_entity) },
                                }
                            },
                        },
                        LinkWith = _entity.GetModule<IMeshModule>(
                            IMeshModule.MODULE_NAME
                        ).Mesh
                    }
                }
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
            await _services.Mediator.Send(
                new ShowGuiCommand(
                    _guiId
                )
            );
        }
    }

    private async Task UpdateDetails()
    {
        await _services.Mediator.Send(
            new UpdateGuiControlCommand(
                _guiId,
                new GuiControlDataModel
                {
                    ControlId = "gui_health_module-bar",
                    Options = new GuiControlOptionsModel
                    {
                        {
                            "__metadata",
                            new GuiControlOptionsModel.GuiControlMetadataOptionModel
                            {
                                ModelOptions = new List<string>{ "textOptions" }
                            }
                        },
                        { "percent", __SCRIPT__Observer.GetEntityPercent(_entity) },
                        {
                            "textOptions",
                            new GuiControlOptionsModel
                            {
                                { "text", __SCRIPT__Observer.GetEntityText(_entity) },
                            }
                        },
                    }
                }
            )
        );
    }

    private async Task UpdateLinkWith()
    {
        System.Console.WriteLine("UpdateLinkWith: " + _entity.Name);
        System.Console.WriteLine("UpdateLinkWith: " + _entity.GetModule<IMeshModule>(
            IMeshModule.MODULE_NAME
        ).Mesh != null);
        await _services.Mediator.Send(
            new UpdateGuiControlCommand(
                _guiId,
                new GuiControlDataModel
                {
                    ControlId = "gui_health_module-bar",
                    LinkWith = _entity.GetModule<IMeshModule>(
                        IMeshModule.MODULE_NAME
                    ).Mesh
                }
            )
        );
    }
}
