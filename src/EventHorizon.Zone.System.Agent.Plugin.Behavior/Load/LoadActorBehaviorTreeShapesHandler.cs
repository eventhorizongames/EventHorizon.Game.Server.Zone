using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.FileService;
using EventHorizon.Zone.Core.Model.FileService;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.Core.Model.Json;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Api;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;
using MediatR;

namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Load
{
    public class LoadActorBehaviorTreeShapesHandler : IRequestHandler<LoadActorBehaviorTreeShapes>
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

        public Task<Unit> Handle(
            LoadActorBehaviorTreeShapes request,
            CancellationToken cancellationToken
        ) => _mediator.Send(
            new LoadFileRecursivelyFromDirectory(
                Path.Combine(
                    _serverInfo.ServerPath,
                    "Behaviors"
                ),
                OnProcessFile,
                new Dictionary<string, object>
                {
                    {
                        "RootPath",
                        $"{_serverInfo.ServerPath}{Path.DirectorySeparatorChar}"
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
            var treeId = GenerateName(
                rootPath.MakePathRelative(
                    fileInfo.DirectoryName
                ), fileInfo.Name
            );
            _actorBehaviorTreeRepository.RegisterTree(
                treeId,
                new ActorBehaviorTreeShape(
                    await _fileLoader.GetFile<SerializedAgentBehaviorTree>(
                        fileInfo.FullName
                    )
                )
            );
        }

        /// <summary>
        /// TODO: Move this to a Model, maybe a NameGenerator abstraction.
        /// </summary>
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