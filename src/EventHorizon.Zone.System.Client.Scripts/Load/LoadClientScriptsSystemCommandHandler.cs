namespace EventHorizon.Zone.System.Client.Scripts.Load
{
    using EventHorizon.Zone.Core.Events.FileService;
    using EventHorizon.Zone.Core.Model.FileService;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.System.Client.Scripts.Api;
    using EventHorizon.Zone.System.Client.Scripts.Model;

    using global::System.Collections.Generic;
    using global::System.IO;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    public class LoadClientScriptsSystemCommandHandler
        : IRequestHandler<LoadClientScriptsSystemCommand>
    {
        private readonly IMediator _mediator;
        private readonly ServerInfo _serverInfo;
        private readonly ClientScriptRepository _clientScriptRepository;

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

        public Task<Unit> Handle(
            LoadClientScriptsSystemCommand request,
            CancellationToken cancellationToken
        ) => _mediator.Send(
            new ProcessFilesRecursivelyFromDirectory(
                _serverInfo.ClientScriptsPath,
                OnProcessFile,
                new Dictionary<string, object>
                {
                    {
                        "RootPath",
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
            if (fileInfo.Extension == ".js")
            {
                scriptType = ClientScriptType.JavaScript;
            }
            else if (fileInfo.Extension == ".csx")
            {
                scriptType = ClientScriptType.CSharp;
            }

            return scriptType;
        }
    }
}
