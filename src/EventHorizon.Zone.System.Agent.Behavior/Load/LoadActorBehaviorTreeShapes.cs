using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.External.Extensions;
using EventHorizon.Game.Server.Zone.External.Info;
using EventHorizon.Game.Server.Zone.External.Json;
using EventHorizon.Zone.System.Agent.Behavior.Api;
using EventHorizon.Zone.System.Agent.Behavior.Model;
using MediatR;

namespace EventHorizon.Zone.System.Agent.Behavior.Load
{
    public struct LoadActorBehaviorTreeShapes : IRequest
    {
        public struct LoadActorBehaviorTreeShapesHandler : IRequestHandler<LoadActorBehaviorTreeShapes>
        {
            readonly IMediator _mediator;
            readonly ServerInfo _serverInfo;
            readonly IJsonFileLoader _fileLoader;
            readonly ActorBehaviorTreeRepository _actorBehaviorTreeRepository;

            public LoadActorBehaviorTreeShapesHandler(
                IMediator mediator,
                ServerInfo serverInfo,
                IJsonFileLoader fileLoader,
                ActorBehaviorTreeRepository actorBehaviorTreeRepository
            )
            {
                _mediator = mediator;
                _serverInfo = serverInfo;
                _fileLoader = fileLoader;
                _actorBehaviorTreeRepository = actorBehaviorTreeRepository;
            }

            public async Task<Unit> Handle(
                LoadActorBehaviorTreeShapes request,
                CancellationToken cancellationToken
            )
            {
                await this.LoadFromDirectoryInfo(
                    _serverInfo.ServerPath,
                    new DirectoryInfo(
                        Path.Combine(
                            _serverInfo.ServerPath,
                            "Behaviors"
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
                foreach (var treeShapeFile in directoryInfo.GetFiles())
                {
                    var treeId = GenerateName(
                        $"{scriptsPath}{Path.DirectorySeparatorChar}".MakePathRelative(
                            treeShapeFile.DirectoryName
                        ), treeShapeFile.Name
                    );
                    _actorBehaviorTreeRepository.RegisterTree(
                        treeId,
                        new ActorBehaviorTreeShape(
                            await _fileLoader.GetFile<SerializedAgentBehaviorTree>(
                                treeShapeFile.FullName
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