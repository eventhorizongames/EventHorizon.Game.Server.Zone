using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.System.Server.Scripts.Events.Load;
using EventHorizon.Zone.System.Server.Scripts.Events.Register;
using MediatR;
using IOPath = System.IO.Path;

namespace EventHorizon.Game.Server.Zone.Server.Load
{
    public struct LoadAdminServerScriptsHandler : IRequestHandler<LoadServerScriptsCommand>
    {
        readonly IMediator _mediator;
        readonly ServerInfo _serverInfo;
        public LoadAdminServerScriptsHandler(
            IMediator mediator,
            ServerInfo serverInfo
        )
        {
            _mediator = mediator;
            _serverInfo = serverInfo;
        }
        public async Task<Unit> Handle(
            LoadServerScriptsCommand request,
            CancellationToken cancellationToken
        )
        {
            // Start Loading Script from Root Client Scripts Directory
            await this.LoadFromDirectoryInfo(
                GetClientScriptsPath() + IOPath.DirectorySeparatorChar,
                new DirectoryInfo(
                    IOPath.Combine(
                        GetClientScriptsPath(),
                        "Admin"
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
            var scriptReferenceAssemblies = new Assembly[] {
                typeof(LoadAdminServerScriptsHandler).Assembly
            };
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

        private string GetClientScriptsPath()
        {
            return IOPath.Combine(
                _serverInfo.ServerPath,
                "Scripts"
            );
        }
    }
}