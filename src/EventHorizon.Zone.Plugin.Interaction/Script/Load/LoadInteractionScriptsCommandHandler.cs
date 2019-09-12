using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.External.Extensions;
using EventHorizon.Game.Server.Zone.External.Info;
using EventHorizon.Zone.System.Server.Scripts.Events.Register;
using MediatR;

namespace EventHorizon.Zone.Plugin.Interaction.Script.Load
{
    public struct LoadInteractionScriptsCommandHandler : IRequestHandler<LoadInteractionScriptsCommand>
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

        public async Task<Unit> Handle(
            LoadInteractionScriptsCommand request,
            CancellationToken cancellationToken
        )
        {
            var interactionPath = Path.Combine(
                _serverInfo.ServerScriptsPath,
                "Interaction"
            );
            // Start Loading Script from Root Client Scripts Directory
            await this.LoadFromDirectoryInfo(
                $"{_serverInfo.ServerScriptsPath}{Path.DirectorySeparatorChar}",
                new DirectoryInfo(
                    interactionPath
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
            foreach (var fileInfo in directoryInfo.GetFiles())
            {
                var scriptReferenceAssemblies = new Assembly[] {
                    typeof(LoadInteractionScriptsCommandHandler).Assembly
                };
                var scriptImports = new string[] {
                };
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
        private static string GenerateName(
            string path,
            string fileName
        )
        {
            return string.Join(
                "_",
                string.Join(
                    "_",
                    path.Split(
                        Path.DirectorySeparatorChar
                    )
                ),
                fileName
            );
        }
    }
}