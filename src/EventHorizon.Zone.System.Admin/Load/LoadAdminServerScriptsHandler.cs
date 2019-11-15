using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.FileService;
using EventHorizon.Zone.Core.Model.FileService;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.System.Server.Scripts.Events.Load;
using EventHorizon.Zone.System.Server.Scripts.Events.Register;
using MediatR;

namespace EventHorizon.Zone.System.Admin.Load
{
    public class LoadAdminServerScriptsHandler : IRequestHandler<LoadServerScriptsCommand>
    {
        readonly IMediator _mediator;
        readonly ServerInfo _serverInfo;
        readonly SystemProvidedAssemblyList _systemAssemblyList;

        public LoadAdminServerScriptsHandler(
            IMediator mediator,
            ServerInfo serverInfo,
            SystemProvidedAssemblyList systemAssemblyList
        )
        {
            _mediator = mediator;
            _serverInfo = serverInfo;
            _systemAssemblyList = systemAssemblyList;
        }

        public Task<Unit> Handle(
            LoadServerScriptsCommand request,
            CancellationToken cancellationToken
        ) => _mediator.Send(
            new LoadFileRecursivelyFromDirectory(
                Path.Combine(
                    _serverInfo.ServerScriptsPath,
                    "Admin"
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
            var scriptReferenceAssemblies = _systemAssemblyList.List;
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