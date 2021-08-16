using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using EventHorizon.Zone.Core.Events.DirectoryService;
using EventHorizon.Zone.Core.Events.Gui;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.Core.Model.Json;
using EventHorizon.Zone.System.Gui.Model;

using MediatR;

namespace EventHorizon.Zone.System.Gui.Load
{
    /// <summary>
    /// TODO: Make this recursive loading
    /// </summary>
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

        public async Task<Unit> Handle(
            LoadSystemGuiCommand request,
            CancellationToken cancellationToken
        )
        {
            // Register Gui Layout and Templates from Files
            foreach (var guiLayout in await GetGuiLayoutFileList(
                Path.Combine(
                    _serverInfo.ClientPath,
                    "Gui"
                )
            ))
            {
                // Register Layout from Gui File
                await _mediator.Send(
                    new RegisterGuiLayoutCommand
                    {
                        Layout = guiLayout
                    }
                );
            }
            return Unit.Value;
        }

        private async Task<IList<GuiLayout>> GetGuiLayoutFileList(
            string guiPath
        )
        {
            var result = new List<GuiLayout>();
            foreach (var fileInfo in await _mediator.Send(
                new GetListOfFilesFromDirectory(
                    guiPath
                )
            ))
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
