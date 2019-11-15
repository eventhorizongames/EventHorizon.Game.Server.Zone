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

namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Load
{
    public class LoadActorBehaviorScriptsHandler : IRequestHandler<LoadActorBehaviorScripts>
    {
        readonly IMediator _mediator;
        readonly ServerInfo _serverInfo;
        readonly SystemProvidedAssemblyList _systemAssemblyList;

        public LoadActorBehaviorScriptsHandler(
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
            LoadActorBehaviorScripts request,
            CancellationToken cancellationToken
        ) => _mediator.Send(
            new LoadFileRecursivelyFromDirectory(
                Path.Combine(
                    _serverInfo.ServerScriptsPath,
                    "Behavior"
                ),
                OnProcessFile,
                new Dictionary<string, object>
                {
                    {
                        "RootPath",
                        _serverInfo.ServerScriptsPath
                    },
                    {
                        "ScriptReferenceAssemblies",
                        _systemAssemblyList.List
                    },
                    {
                        "ScriptImports",
                        new string[] { }
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
            var scriptReferenceAssemblies = arguments["ScriptReferenceAssemblies"] as IList<Assembly>;
            var scriptImports = arguments["ScriptImports"] as string[];
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