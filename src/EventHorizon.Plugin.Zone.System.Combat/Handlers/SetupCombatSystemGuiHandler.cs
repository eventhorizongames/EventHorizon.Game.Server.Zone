using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Events.Gui;
using EventHorizon.Game.Server.Zone.External.Info;
using EventHorizon.Game.Server.Zone.External.Json;
using EventHorizon.Game.Server.Zone.Model.Gui;
using EventHorizon.Game.Server.Zone.Model.Gui.Templates;
using EventHorizon.Plugin.Zone.System.Combat.Events;
using EventHorizon.Plugin.Zone.System.Combat.Model.Gui;
using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Handlers
{
    public class SetupCombatSystemGuiHandler : INotificationHandler<SetupCombatSystemGuiEvent>
    {
        readonly IMediator _mediator;
        readonly IJsonFileLoader _fileLoader;
        readonly ServerInfo _serverInfo;
        public SetupCombatSystemGuiHandler(IMediator mediator, IJsonFileLoader fileLoader, ServerInfo serverInfo)
        {
            _mediator = mediator;
            _fileLoader = fileLoader;
            _serverInfo = serverInfo;
        }
        public async Task Handle(SetupCombatSystemGuiEvent notification, CancellationToken cancellationToken)
        {
            var filePath = Path.Combine(_serverInfo.PluginsPath, "Gui.System.Combat.json");
            var guiList = await _fileLoader.GetFile<CombatSystemGuiControlList>(filePath);
            var template = guiList.MainGrid;

            await _mediator.Publish(new RegisterGuiLayoutEvent
            {
                Layout = guiList.Layout
            });

            await _mediator.Publish(new RegisterControlTemplateEvent
            {
                Id = guiList.MainGrid.Id,
                Template = guiList.MainGrid
            });

            await _mediator.Publish(new RegisterControlTemplateEvent
            {
                Id = guiList.LifePanel.Id,
                Template = guiList.LifePanel
            });

            await _mediator.Publish(new RegisterControlTemplateEvent
            {
                Id = guiList.HealthLabel.Id,
                Template = guiList.HealthLabel
            });

            await _mediator.Publish(new RegisterControlTemplateEvent
            {
                Id = guiList.HealthBar.Id,
                Template = guiList.HealthBar
            });

            await _mediator.Publish(new RegisterControlTemplateEvent
            {
                Id = guiList.MagicLabel.Id,
                Template = guiList.MagicLabel
            });

            await _mediator.Publish(new RegisterControlTemplateEvent
            {
                Id = guiList.MagicBar.Id,
                Template = guiList.MagicBar
            });
        }
    }
}