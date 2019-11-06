using System.IO;
using System.Threading;
using System.Threading.Tasks;
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

        public async Task<Unit> Handle(
            LoadActorBehaviorScripts request,
            CancellationToken cancellationToken
        )
        {
            await this.LoadFromDirectoryInfo(
                Path.Combine(
                    _serverInfo.ServerPath,
                    "Scripts"
                ) + Path.DirectorySeparatorChar,
                new DirectoryInfo(
                    Path.Combine(
                        _serverInfo.ServerPath,
                        "Scripts",
                        "Behavior"
                    )
                )
            );

            return Unit.Value;
        }

        private async Task LoadFromDirectoryInfo(
            string scriptsPath,
            DirectoryInfo directoryInfo
        )
        {
            // Load Scripts from Sub-Directories
            foreach (var subDirectoryInfo in directoryInfo.GetDirectories())
            {
                // Load Files From Directories
                await this.LoadFromDirectoryInfo(
                    scriptsPath,
                    subDirectoryInfo
                );
            }
            // Load script files into Repository
            await this.LoadFileIntoRepository(
                scriptsPath,
                directoryInfo
            );
        }
        private async Task LoadFileIntoRepository(
            string scriptsPath,
            DirectoryInfo directoryInfo
        )
        {
            var scriptReferenceAssemblies = _systemAssemblyList.List;
            var scriptImports = new string[] {
            };
            foreach (var fileInfo in directoryInfo.GetFiles())
            {
                // Register Script with Platform
                await _mediator.Send(
                    new RegisterServerScriptCommand(
                        fileInfo.Name,
                        scriptsPath.MakePathRelative(
                            fileInfo.DirectoryName
                        ),
                        File.ReadAllText(
                            fileInfo.FullName
                        ),
                        scriptReferenceAssemblies,
                        scriptImports
                    )
                );
            }
        }
    }
}