using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.External.Extensions;
using EventHorizon.Game.Server.Zone.External.Info;
using EventHorizon.Game.Server.Zone.External.Json;
using EventHorizon.Zone.System.Agent.Behavior.Api;
using EventHorizon.Zone.System.Agent.Behavior.Script;
using MediatR;

namespace EventHorizon.Zone.System.Agent.Behavior.Load
{
    public struct LoadAgentBehaviorSystemEvent : IRequest
    {
        public struct LoadServerModuleSystemHandler : IRequestHandler<LoadAgentBehaviorSystemEvent>
        {
            readonly IMediator _mediator;
            readonly ServerInfo _serverInfo;
            readonly IJsonFileLoader _fileLoader;
            readonly AgentBehaviorScriptRepository _agentBehaviorScriptRepository;

            public LoadServerModuleSystemHandler(
                IMediator mediator,
                ServerInfo serverInfo,
                IJsonFileLoader fileLoader,
                AgentBehaviorScriptRepository agentBehaviorScriptRepository
            )
            {
                _mediator = mediator;
                _serverInfo = serverInfo;
                _fileLoader = fileLoader;
                _agentBehaviorScriptRepository = agentBehaviorScriptRepository;
            }

            public Task<Unit> Handle(
                LoadAgentBehaviorSystemEvent notification,
                CancellationToken cancellationToken
            )
            {
                this.LoadFromDirectoryInfo(
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
                foreach (var effectFile in directoryInfo.GetFiles())
                {
                    _agentBehaviorScriptRepository.Add(
                        BehaviorScript.CreateScript(
                            GenerateName(
                                $"{scriptsPath}{Path.DirectorySeparatorChar}".MakePathRelative(
                                    effectFile.DirectoryName
                                ), effectFile.Name
                            ),
                            File.ReadAllText(
                                effectFile.FullName
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