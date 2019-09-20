using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Model.Extensions;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.Core.Model.Json;
using EventHorizon.Zone.System.Agent.Behavior.Api;
using EventHorizon.Zone.System.Agent.Behavior.Script.Builder;
using MediatR;

namespace EventHorizon.Zone.System.Agent.Behavior.Load
{
    public struct LoadActorBehaviorScripts : IRequest
    {
        public struct LoadActorBehaviorScriptsHandler : IRequestHandler<LoadActorBehaviorScripts>
        {
            readonly IMediator _mediator;
            readonly ServerInfo _serverInfo;
            readonly IJsonFileLoader _fileLoader;
            readonly ActorBehaviorScriptRepository _agentBehaviorScriptRepository;

            public LoadActorBehaviorScriptsHandler(
                IMediator mediator,
                ServerInfo serverInfo,
                IJsonFileLoader fileLoader,
                ActorBehaviorScriptRepository agentBehaviorScriptRepository
            )
            {
                _mediator = mediator;
                _serverInfo = serverInfo;
                _fileLoader = fileLoader;
                _agentBehaviorScriptRepository = agentBehaviorScriptRepository;
            }

            public async Task<Unit> Handle(
                LoadActorBehaviorScripts request,
                CancellationToken cancellationToken
            )
            {
                _agentBehaviorScriptRepository.Clear();
                await this.LoadFromDirectoryInfo(
                    Path.Combine(
                        _serverInfo.ServerPath,
                        "Scripts"
                    ),
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
                foreach (var effectFile in directoryInfo.GetFiles())
                {
                    var id = GenerateName(
                        $"{scriptsPath}{Path.DirectorySeparatorChar}".MakePathRelative(
                            effectFile.DirectoryName
                        ), effectFile.Name
                    );
                    var content = File.ReadAllText(
                        effectFile.FullName
                    );
                    _agentBehaviorScriptRepository.Add(
                        await _mediator.Send(
                            new BuildBehaviorScript(
                                GenerateName(
                                    $"{scriptsPath}{Path.DirectorySeparatorChar}".MakePathRelative(
                                        effectFile.DirectoryName
                                    ), effectFile.Name
                                ),
                                File.ReadAllText(
                                    effectFile.FullName
                                )
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
}