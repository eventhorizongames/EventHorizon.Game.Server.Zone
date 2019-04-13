using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Events.Gui;
using EventHorizon.Game.Server.Zone.External.Info;
using EventHorizon.Game.Server.Zone.External.Json;
using EventHorizon.Game.Server.Zone.Model.Gui;
using MediatR;

namespace EventHorizon.Zone.System.Gui.Load
{
    public class LoadSystemGuiCommandHandler : AsyncRequestHandler<LoadSystemGuiCommand>
    {
        readonly IMediator _mediator;
        readonly IJsonFileLoader _fileLoader;
        readonly ServerInfo _serverInfo;
        public LoadSystemGuiCommandHandler(
            IMediator mediator,
            IJsonFileLoader fileLoader,
            ServerInfo serverInfo
        )
        {
            _mediator = mediator;
            _fileLoader = fileLoader;
            _serverInfo = serverInfo;
        }
        protected override async Task Handle(LoadSystemGuiCommand request, CancellationToken cancellationToken)
        {
            // Register Gui Layout and Templates from Files
            foreach (var guiLayout in await GetGuiLayoutFileList(GetGuiFilesPath()))
            {
                // Register Layout from Gui File
                await _mediator.Publish(new RegisterGuiLayoutEvent
                {
                    Layout = guiLayout.Layout
                });
                foreach (var template in guiLayout.TemplateList)
                {
                    await _mediator.Publish(new RegisterGuiTemplateEvent
                    {
                        Template = template
                    });
                }
            }
        }
        private string GetGuiFilesPath()
        {
            return Path.Combine(
                _serverInfo.AssetsPath,
                "Gui"
            );
        }
        private async Task<IList<GuiLayoutFile>> GetGuiLayoutFileList(
            string guiPath
        )
        {
            var result = new List<GuiLayoutFile>();
            var directoryInfo = new DirectoryInfo(guiPath);
            foreach (var fileInfo in directoryInfo.GetFiles())
            {
                result.Add(
                    await _fileLoader.GetFile<GuiLayoutFile>(
                        fileInfo.FullName
                    )
                );
            }
            return result;
        }
    }
}