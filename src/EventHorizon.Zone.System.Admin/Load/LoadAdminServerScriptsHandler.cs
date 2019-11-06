using System.IO;
using System.Threading;
using System.Threading.Tasks;
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

        public async Task<Unit> Handle(
            LoadServerScriptsCommand request,
            CancellationToken cancellationToken
        )
        {
            // Start Loading Script from Root Client Scripts Directory
            await this.LoadFromDirectoryInfo(
                _serverInfo.ServerScriptsPath + Path.DirectorySeparatorChar,
                new DirectoryInfo(
                    Path.Combine(
                        _serverInfo.ServerScriptsPath,
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