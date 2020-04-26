using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.FileService;
using EventHorizon.Zone.Core.Model.FileService;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.System.Server.Scripts.Events.Register;
using MediatR;

namespace EventHorizon.Zone.System.Interaction.Script.Load
{
    public class LoadInteractionScriptsCommandHandler : IRequestHandler<LoadInteractionScriptsCommand>
    {
        readonly IMediator _mediator;
        readonly ServerInfo _serverInfo;

        public LoadInteractionScriptsCommandHandler(
            IMediator mediator,
            ServerInfo serverInfo
        )
        {
            _mediator = mediator;
            _serverInfo = serverInfo;
        }

        public Task<Unit> Handle(
            LoadInteractionScriptsCommand request,
            CancellationToken cancellationToken
        ) => _mediator.Send(
            new ProcessFilesRecursivelyFromDirectory(
                Path.Combine(
                    _serverInfo.ServerScriptsPath,
                    "Interaction"
                ),
                OnProcessFile,
                new Dictionary<string, object>
                {
                    {
                        "RootPath",
                        $"{_serverInfo.ServerScriptsPath}{Path.DirectorySeparatorChar}"
                    }
                }
            )
        );

        private async Task OnProcessFile(
            StandardFileInfo fileInfo,
            IDictionary<string, object> arguments
        )
        {
            var rootPath = arguments["RootPath"] as string;
            var scriptReferenceAssemblies = new Assembly[] {
                typeof(LoadInteractionScriptsCommandHandler).Assembly // TODO: Update this to SystemProvidedAssemblyList
            };
            var scriptImports = new string[] {
            };
            // Register Script with Platform
            await _mediator.Send(
                new RegisterServerScriptCommand(
                    fileInfo.Name,
                    rootPath.MakePathRelative(
                        fileInfo.DirectoryName
                    ),
                    await _mediator.Send(
                        new ReadAllTextFromFile(
                            fileInfo.FullName
                        )
                    ),
                    scriptReferenceAssemblies,
                    scriptImports
                )
            );
        }
    }
}