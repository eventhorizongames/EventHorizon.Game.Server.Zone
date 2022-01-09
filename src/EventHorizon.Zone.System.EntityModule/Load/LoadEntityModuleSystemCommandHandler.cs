namespace EventHorizon.Zone.System.EntityModule.Load;

using EventHorizon.Zone.Core.Events.DirectoryService;
using EventHorizon.Zone.Core.Model.Command;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.Core.Model.Json;
using EventHorizon.Zone.System.EntityModule.Api;
using EventHorizon.Zone.System.EntityModule.Model;

using global::System.Collections.Generic;
using global::System.IO;
using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

public class LoadEntityModuleSystemCommandHandler
    : IRequestHandler<LoadEntityModuleSystemCommand, StandardCommandResult>
{
    private readonly ISender _sender;
    private readonly ServerInfo _serverInfo;
    private readonly IJsonFileLoader _fileLoader;
    private readonly EntityModuleRepository _entityModuleRepository;

    public LoadEntityModuleSystemCommandHandler(
        ISender sender,
        ServerInfo serverInfo,
        IJsonFileLoader fileLoader,
        EntityModuleRepository entityModuleRepository
    )
    {
        _sender = sender;
        _serverInfo = serverInfo;
        _fileLoader = fileLoader;
        _entityModuleRepository = entityModuleRepository;
    }

    public async Task<StandardCommandResult> Handle(
        LoadEntityModuleSystemCommand notification,
        CancellationToken cancellationToken
    )
    {
        // Load BaseModule list into repository
        foreach (var baseModule in await GetModuleList(
            GetModulePathForType("Base"),
            cancellationToken
        ))
        {
            _entityModuleRepository.AddBaseModule(
                baseModule
            );
        }

        // Load PlayerModule list into repository
        foreach (var playerModule in await GetModuleList(
            GetModulePathForType("Player"),
            cancellationToken
        ))
        {
            _entityModuleRepository.AddPlayerModule(
                playerModule
            );
        }

        return new();
    }

    private string GetModulePathForType(
        string type
    )
    {
        return Path.Combine(
            _serverInfo.ClientPath,
            "Modules",
            type
        );
    }

    private async Task<IList<EntityScriptModule>> GetModuleList(
        string modulePath,
        CancellationToken cancellationToken
    )
    {
        var result = new List<EntityScriptModule>();
        var files = await _sender.Send(
            new GetListOfFilesFromDirectory(
                modulePath
            ),
            cancellationToken
        );

        foreach (var file in files)
        {
            var script = await _fileLoader.GetFile<EntityScriptModule>(
                file.FullName
            );
            if (script.IsNull())
            {
                continue;
            }

            result.Add(
                script
            );
        }
        return result;
    }
}
