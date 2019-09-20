
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Model.Extensions;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.Core.Model.Json;
using EventHorizon.Zone.System.Agent.Behavior.Api;
using EventHorizon.Zone.System.Agent.Behavior.Model;
using EventHorizon.Zone.System.Agent.Behavior.Script.Builder;
using MediatR;

namespace EventHorizon.Zone.System.Agent.Behavior.Load
{
    public struct LoadDefaultActorBehaviorTree : IRequest
    {
        public struct LoadDefaultActorBehaviorTreeHandler : IRequestHandler<LoadDefaultActorBehaviorTree>
        {
            readonly IMediator _mediator;
            readonly ServerInfo _serverInfo;
            readonly IJsonFileLoader _fileLoader;
            readonly ActorBehaviorTreeRepository _actorBehaviorTreeRepository;
            readonly ActorBehaviorScriptRepository _agentBehaviorScriptRepository;

            public LoadDefaultActorBehaviorTreeHandler(
                IMediator mediator,
                ServerInfo serverInfo,
                IJsonFileLoader fileLoader,
                ActorBehaviorTreeRepository actorBehaviorTreeRepository,
                ActorBehaviorScriptRepository agentBehaviorScriptRepository
            )
            {
                _mediator = mediator;
                _serverInfo = serverInfo;
                _fileLoader = fileLoader;
                _actorBehaviorTreeRepository = actorBehaviorTreeRepository;
                _agentBehaviorScriptRepository = agentBehaviorScriptRepository;
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
                        await _fileLoader.GetFile<SerializedAgentBehaviorTree>(
                            Path.Combine(
                                _serverInfo.SystemPath,
                                "Behaviors",
                                "$DEFAULT$SHAPE.json"
                            )
                        )
                    )
                );

                // Load in the System Default Script
                _agentBehaviorScriptRepository.Add(
                    await _mediator.Send(
                        new BuildBehaviorScript(
                            "$DEFAULT$SCRIPT",
                            File.ReadAllText(
                                Path.Combine(
                                    _serverInfo.SystemPath,
                                    "Behaviors",
                                    "$DEFAULT$SCRIPT.csx"
                                )
                            )
                        )
                    )
                );

                return Unit.Value;
            }
        }
    }
}