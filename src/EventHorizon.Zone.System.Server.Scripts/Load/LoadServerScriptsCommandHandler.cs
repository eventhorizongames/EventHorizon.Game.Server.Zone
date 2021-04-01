namespace EventHorizon.Zone.System.Server.Scripts.Load
{
    using EventHorizon.Zone.Core.Events.FileService;
    using EventHorizon.Zone.Core.Model.FileService;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.System.Server.Scripts.Model.Details;
    using EventHorizon.Zone.System.Server.Scripts.Set;
    using EventHorizon.Zone.System.Server.Scripts.State;
    using global::System.Collections.Generic;
    using global::System.IO;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;

    public class LoadServerScriptsCommandHandler
        : IRequestHandler<LoadServerScriptsCommand>
    {
        private readonly IMediator _mediator;
        private readonly ServerInfo _serverInfo;
        private readonly ServerScriptDetailsRepository _detailsRepository;

        public LoadServerScriptsCommandHandler(
            IMediator mediator,
            ServerInfo serverInfo,
            ServerScriptDetailsRepository detailsRepository
        )
        {
            _mediator = mediator;
            _serverInfo = serverInfo;
            _detailsRepository = detailsRepository;
        }

        public async Task<Unit> Handle(
            LoadServerScriptsCommand request,
            CancellationToken cancellationToken
        )
        {
            _detailsRepository.Clear();

            await _mediator.Send(
                new ProcessFilesRecursivelyFromDirectory(
                    _serverInfo.ServerScriptsPath,
                    OnProcessFile,
                    new Dictionary<string, object>
                    {
                        ["RootPath"] = _serverInfo.ServerScriptsPath,
                    }
                ),
                cancellationToken
            );

            await _mediator.Send(
                new ProcessFilesRecursivelyFromDirectory(
                    _serverInfo.SystemsPath,
                    OnProcessFile,
                    new Dictionary<string, object>
                    {
                        ["RootPath"] = _serverInfo.SystemsPath,
                    }
                ),
                cancellationToken
            );

            return Unit.Value;
        }

        private async Task OnProcessFile(
            StandardFileInfo fileInfo,
            IDictionary<string, object> arguments
        )
        {
            if (fileInfo.Extension != ".csx")
            {
                return;
            }

            var rootPath = arguments["RootPath"] as string;

            // Register Script with Platform
            await _mediator.Send(
                new SetServerScriptDetailsCommand(
                    new ServerScriptDetails(
                        fileInfo.Name.Replace(".csx", string.Empty),
                        rootPath.MakePathRelative(
                            fileInfo.DirectoryName
                        ),
                        await _mediator.Send(
                            new ReadAllTextFromFile(
                                fileInfo.FullName
                            )
                        )
                    )
                )
            );
        }
    }
}
