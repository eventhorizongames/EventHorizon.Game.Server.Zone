namespace EventHorizon.Zone.System.Player.Editor;

using EventHorizon.Zone.Core.Model.Command;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.Core.Model.Json;
using EventHorizon.Zone.System.Player.Api;
using EventHorizon.Zone.System.Player.Model;
using global::System.IO;
using global::System.Threading;
using global::System.Threading.Tasks;
using MediatR;

public class SaveEditorPlayerDataHandler(ServerInfo serverInfo, IJsonFileSaver fileSaver)
    : IRequestHandler<SaveEditorPlayerData, StandardCommandResult>
{
    public async Task<StandardCommandResult> Handle(
        SaveEditorPlayerData request,
        CancellationToken cancellationToken
    )
    {
        // TODO: Validate Player Data
        var directoryFullName = Path.Combine(
            serverInfo.AppDataPath,
            PlayerSystemConstants.PlayerAppDataPath
        );

        await fileSaver.SaveToFile(
            directoryFullName,
            PlayerSystemConstants.PlayerDataFileName,
            request.PlayerData
        );

        return new();
    }
}
