using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.External.Extensions;
using EventHorizon.Game.Server.Zone.External.Info;
using EventHorizon.Zone.Plugin.Interaction.Script.Model;
using EventHorizon.Zone.Plugin.Interaction.Script.State;
using MediatR;

namespace EventHorizon.Zone.Plugin.Interaction.Script.Load
{
    public class LoadInteractionScriptsCommandHandler : IRequestHandler<LoadInteractionScriptsCommand>
    {
        readonly ServerInfo _serverInfo;
        readonly InteractionScriptRepository _interactionScriptRepository;

        public LoadInteractionScriptsCommandHandler(
            ServerInfo serverInfo,
            InteractionScriptRepository interactionScriptRepository
        )
        {
            _serverInfo = serverInfo;
            _interactionScriptRepository = interactionScriptRepository;
        }

        public Task<Unit> Handle(
            LoadInteractionScriptsCommand request,
            CancellationToken cancellationToken
        )
        {
            var interactionPath = Path.Combine(
                _serverInfo.ServerScriptsPath,
                "Interaction"
            );
            // Start Loading Script from Root Client Scripts Directory
            this.LoadFromDirectoryInfo(
                _serverInfo.ServerScriptsPath,
                new DirectoryInfo(
                    interactionPath
                )
            );
            return Unit.Task;
        }

        private void LoadFromDirectoryInfo(
            string scriptsPath,
            DirectoryInfo directoryInfo
        )
        {
            // Load Scripts from Sub-Directories
            foreach (var subDirectoryInfo in directoryInfo.GetDirectories())
            {
                // Load Files From Directories
                this.LoadFromDirectoryInfo(
                    scriptsPath,
                    subDirectoryInfo
                );
            }
            // Load script files into Repository
            this.LoadFileIntoRepository(
                scriptsPath,
                directoryInfo
            );
        }


        private void LoadFileIntoRepository(
            string scriptsPath,
            DirectoryInfo directoryInfo
        )
        {
            foreach (var fileInfo in directoryInfo.GetFiles())
            {
                // Create CSXInteractionScript AND Set to Repository
                _interactionScriptRepository.Set(
                    CSXInteractionScript.Create(
                        GenerateName(
                            $"{scriptsPath}{Path.DirectorySeparatorChar}".MakePathRelative(
                                fileInfo.DirectoryName
                            ), fileInfo.Name
                        ),
                        File.ReadAllText(
                            fileInfo.FullName
                        )
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