using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Events.Gui;
using EventHorizon.Game.Server.Zone.External.Info;
using EventHorizon.Game.Server.Zone.External.Json;
using EventHorizon.Game.Server.Zone.Model.Gui;
using EventHorizon.Plugin.Zone.System.Combat.Events;
using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Gui
{
    public class LoadCombatSystemGuiHandler : INotificationHandler<LoadCombatSystemGuiEvent>
    {
        readonly IMediator _mediator;
        readonly IJsonFileLoader _fileLoader;
        readonly ServerInfo _serverInfo;
        public LoadCombatSystemGuiHandler(IMediator mediator, IJsonFileLoader fileLoader, ServerInfo serverInfo)
        {
            _mediator = mediator;
            _fileLoader = fileLoader;
            _serverInfo = serverInfo;
        }
        public async Task Handle(LoadCombatSystemGuiEvent notification, CancellationToken cancellationToken)
        {
            var filePath = Path.Combine(
                _serverInfo.AssetsPath,
                "Gui",
                notification.FileName
            );

            // Load file
            var gui = await _fileLoader.GetFile<GuiLayoutFile>(filePath);
            // Register Layout from Gui File
            await _mediator.Publish(new RegisterGuiLayoutEvent
            {
                Layout = gui.Layout
            });

            // Register Gui controls from Gui File
            foreach (var template in gui.TemplateList)
            {
                await _mediator.Publish(new RegisterGuiTemplateEvent
                {
                    Template = template
                });

            }
        }
    }
}