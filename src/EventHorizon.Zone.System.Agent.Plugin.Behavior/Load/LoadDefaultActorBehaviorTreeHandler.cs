namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Load
{
    using global::System.IO;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using EventHorizon.Zone.Core.Events.FileService;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.Core.Model.Json;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.Api;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;
    using EventHorizon.Zone.System.Server.Scripts.Events.Register;
    using MediatR;

    public class LoadDefaultActorBehaviorTreeHandler : IRequestHandler<LoadDefaultActorBehaviorTree>
    {
        readonly IMediator _mediator;
        readonly ServerInfo _serverInfo;
        readonly IJsonFileLoader _fileLoader;
        readonly ActorBehaviorTreeRepository _actorBehaviorTreeRepository;
        readonly SystemProvidedAssemblyList _systemAssemblyList;

        public LoadDefaultActorBehaviorTreeHandler(
            IMediator mediator,
            ServerInfo serverInfo,
            IJsonFileLoader fileLoader,
            ActorBehaviorTreeRepository actorBehaviorTreeRepository,
            SystemProvidedAssemblyList systemAssemblyList
        )
        {
            _mediator = mediator;
            _serverInfo = serverInfo;
            _fileLoader = fileLoader;
            _actorBehaviorTreeRepository = actorBehaviorTreeRepository;
            _systemAssemblyList = systemAssemblyList;
        }

        public async Task<Unit> Handle(
            LoadDefaultActorBehaviorTree request,
            CancellationToken cancellationToken
        )
        {
            // Load in the System Default Tree Shape
            _actorBehaviorTreeRepository.RegisterTree(
                "DEFAULT",
                new ActorBehaviorTreeShape(
                    "DEFAULT",
                    await _fileLoader.GetFile<SerializedAgentBehaviorTree>(
                        Path.Combine(
                            _serverInfo.SystemPath,
                            "Behaviors",
                            "$DEFAULT$SHAPE.json"
                        )
                    )
                )
            );

            // Register Default Script with Server
            var defaultScriptName = "$DEFAULT$SCRIPT";
            await _mediator.Send(
                new RegisterServerScriptCommand(
                    defaultScriptName,
                    string.Empty,
                    await _mediator.Send(
                        new ReadAllTextFromFile(
                            Path.Combine(
                                _serverInfo.SystemPath,
                                "Behaviors",
                                $"{defaultScriptName}.csx"
                            )
                        )
                    ),
                    _systemAssemblyList.List,
                    new string[] { }
                )
            );

            return Unit.Value;
        }
    }
}