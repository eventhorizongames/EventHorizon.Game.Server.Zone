using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Events.Gui;
using EventHorizon.Game.Server.Zone.External.Info;
using EventHorizon.Game.Server.Zone.External.Json;
using EventHorizon.Zone.System.Gui.Model;
using MediatR;

namespace EventHorizon.Zone.System.Gui.Load
{
    public class LoadSystemGuiCommandHandler : IRequestHandler<LoadSystemGuiCommand>
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
        public async Task<Unit> Handle(LoadSystemGuiCommand request, CancellationToken cancellationToken)
        {
            // Register Gui Layout and Templates from Files
            foreach (var guiLayout in await GetGuiLayoutFileList(GetGuiFilesPath()))
            {
                // Register Layout from Gui File
                await _mediator.Send(new RegisterGuiLayoutCommand
                {
                    Layout = guiLayout
                });
            }
            return Unit.Value;
        }
        private string GetGuiFilesPath()
        {
            return Path.Combine(
                _serverInfo.ClientPath,
                "Gui"
            );
        }
        private async Task<IList<GuiLayout>> GetGuiLayoutFileList(
            string guiPath
        )
        {
            var result = new List<GuiLayout>();
            var directoryInfo = new DirectoryInfo(guiPath);
            foreach (var fileInfo in directoryInfo.GetFiles())
            {
                result.Add(
                    await _fileLoader.GetFile<GuiLayout>(
                        fileInfo.FullName
                    )
                );
            }
            return result;
        }
    }
}