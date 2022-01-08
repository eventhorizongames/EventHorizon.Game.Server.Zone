namespace EventHorizon.Zone.System.Gui.Load;

using EventHorizon.Zone.Core.Events.DirectoryService;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.Core.Model.Json;
using EventHorizon.Zone.System.Gui.Model;
using EventHorizon.Zone.System.Gui.Register;

using global::System.Collections.Generic;
using global::System.IO;
using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

/// <summary>
/// TODO: Make this recursive loading
/// </summary>
public class LoadSystemGuiCommandHandler
    : IRequestHandler<LoadSystemGuiCommand>
{
    private readonly IMediator _mediator;
    private readonly IJsonFileLoader _fileLoader;
    private readonly ServerInfo _serverInfo;

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
            ),
            cancellationToken
        ))
        {
            // Register Layout from Gui File
            await _mediator.Send(
                new RegisterGuiLayoutCommand(
                    guiLayout
                ),
                cancellationToken
            );
        }
        return Unit.Value;
    }

    private async Task<IList<GuiLayout>> GetGuiLayoutFileList(
        string guiPath,
        CancellationToken cancellationToken
    )
    {
        var result = new List<GuiLayout>();
        foreach (var fileInfo in await _mediator.Send(
            new GetListOfFilesFromDirectory(
                guiPath
            ),
            cancellationToken
        ))
        {
            var layout = await _fileLoader.GetFile<GuiLayout>(
                fileInfo.FullName
            );
            if (layout.IsNull())
            {
                continue;
            }

            result.Add(
                layout
            );
        }
        return result;
    }
}
