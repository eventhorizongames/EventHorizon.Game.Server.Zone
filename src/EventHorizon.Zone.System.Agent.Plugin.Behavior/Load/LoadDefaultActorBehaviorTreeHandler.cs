namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Load;

using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.Core.Model.Json;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Api;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;
using global::System.IO;
using global::System.Threading;
using global::System.Threading.Tasks;
using MediatR;

public class LoadDefaultActorBehaviorTreeHandler : IRequestHandler<LoadDefaultActorBehaviorTree>
{
    private readonly ServerInfo _serverInfo;
    private readonly IJsonFileLoader _fileLoader;
    private readonly ActorBehaviorTreeRepository _actorBehaviorTreeRepository;

    public LoadDefaultActorBehaviorTreeHandler(
        ServerInfo serverInfo,
        IJsonFileLoader fileLoader,
        ActorBehaviorTreeRepository actorBehaviorTreeRepository
    )
    {
        _serverInfo = serverInfo;
        _fileLoader = fileLoader;
        _actorBehaviorTreeRepository = actorBehaviorTreeRepository;
    }

    public async Task Handle(
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
                        _serverInfo.SystemsPath,
                        "Agent",
                        "Plugin",
                        "Behavior",
                        "Behaviors",
                        "$DEFAULT$SHAPE.json"
                    )
                )
            )
        );
    }
}
