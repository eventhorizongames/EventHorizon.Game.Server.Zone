namespace EventHorizon.Zone.System.Player.Editor;

using global::System;
using EventHorizon.Zone.Core.Model.Command;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.Core.Model.Json;
using EventHorizon.Zone.System.Player.Api;
using EventHorizon.Zone.System.Player.Model;
using EventHorizon.Zone.System.Player.Model.Settings;
using global::System.IO;
using global::System.Threading;
using global::System.Threading.Tasks;
using MediatR;

public class QueryForEditorPlayerDataHandler(ServerInfo serverInfo, IJsonFileLoader fileLoader)
    : IRequestHandler<QueryForEditorPlayerData, CommandResult<ObjectEntityData>>
{
    public async Task<CommandResult<ObjectEntityData>> Handle(
        QueryForEditorPlayerData request,
        CancellationToken cancellationToken
    )
    {
        var fileFullName = Path.Combine(
            serverInfo.AppDataPath,
            PlayerSystemConstants.PlayerAppDataPath,
            PlayerSystemConstants.PlayerDataFileName
        );

        var data = await fileLoader.GetFile<PlayerObjectEntityDataModel>(fileFullName);
        if (data is null)
        {
            return "player_data_not_found";
        }

        return data;
    }
}
