using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.FileService;
using EventHorizon.Zone.Core.Model.FileService;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.System.Client.Scripts.Model;
using EventHorizon.Zone.System.Client.Scripts.State;
using MediatR;

namespace EventHorizon.Zone.System.Client.Scripts.Load
{
    public class LoadClientScriptsSystemCommandHandler : INotificationHandler<LoadClientScriptsSystemCommand>
    {
        readonly IMediator _mediator;
        readonly ServerInfo _serverInfo;
        readonly ClientScriptRepository _clientScriptRepository;

        public LoadClientScriptsSystemCommandHandler(
            IMediator mediator,
            ServerInfo serverInfo,
            ClientScriptRepository clientScriptRepository
        )
        {
            _mediator = mediator;
            _serverInfo = serverInfo;
            _clientScriptRepository = clientScriptRepository;
        }

        public Task Handle(
            LoadClientScriptsSystemCommand notification,
            CancellationToken cancellationToken
        ) => _mediator.Send(
            new ProcessFilesRecursivelyFromDirectory(
                _serverInfo.ClientScriptsPath,
                OnProcessFile,
                new Dictionary<string, object>
                {
                    {
                        "RootPath",
                        // $"{_serverInfo.ClientScriptsPath}{Path.DirectorySeparatorChar}"
                        _serverInfo.ClientScriptsPath
                    }
                }
            )
        );

        private async Task OnProcessFile(
            StandardFileInfo fileInfo,
            IDictionary<string, object> arguments
        )
        {
            var scriptType = GetScriptType(
                fileInfo
            );
            var rootPath = arguments["RootPath"] as string;
            // Create ClientScript AND Add to Repository
            _clientScriptRepository.Add(
                ClientScript.Create(
                    scriptType,
                    rootPath.MakePathRelative(
                        fileInfo.DirectoryName
                    ),
                    fileInfo.Name,
                    await _mediator.Send(
                        new ReadAllTextFromFile(
                            fileInfo.FullName
                        )
                    )
                )
            );
        }

        private static ClientScriptType GetScriptType(
            StandardFileInfo fileInfo
        )
        {
            var scriptType = ClientScriptType.Unknown;
            if (fileInfo.Name.EndsWith(".js"))
            {
                scriptType = ClientScriptType.JavaScript;
            }
            else if (fileInfo.Name.EndsWith(".csx"))
            {
                scriptType = ClientScriptType.CSharp;
            }

            return scriptType;
        }
    }
}