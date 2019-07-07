using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.External.Info;
using EventHorizon.Game.Server.Zone.External.Json;
using EventHorizon.Zone.System.Agent.Behavior.Api;
using EventHorizon.Zone.System.Agent.Behavior.Load;
using EventHorizon.Zone.System.Agent.Behavior.Model;
using EventHorizon.Zone.System.Agent.Behavior.Script.Builder;
using MediatR;
using Moq;
using Xunit;
using static EventHorizon.Zone.System.Agent.Behavior.Load.LoadDefaultActorBehaviorTree;

namespace EventHorizon.Game.Server.Zone.Tests.Agent.Behavior.Load
{
    public class LoadDefaultActorBehaviorTreeHandlerTests
    {
        [Fact]
        public async Task ShouldRegisterDefaultBehaviorTreeShapeWhenCalled()
        {
            // Given
            var systemPath = System.IO.Path.Combine(
                System.AppDomain.CurrentDomain.BaseDirectory,
                "Agent",
                "Behavior",
                "Load",
                "System"
            );
            var defaultBehaviorShapePath = System.IO.Path.Combine(
                systemPath,
                "Behaviors",
                "$DEFAULT$SHAPE.json"
            );

            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();
            var fileLoaderMock = new Mock<IJsonFileLoader>();
            var actorBehaviorTreeRepositoryMock = new Mock<ActorBehaviorTreeRepository>();
            var actorBehaviorScriptRepositoryMock = new Mock<ActorBehaviorScriptRepository>();

            serverInfoMock.SetupGet(
                serverInfo => serverInfo.SystemPath
            ).Returns(
                systemPath
            );

            fileLoaderMock.Setup(
                fileLoader => fileLoader.GetFile<SerializedAgentBehaviorTree>(
                    defaultBehaviorShapePath
                )
            ).ReturnsAsync(
                new SerializedAgentBehaviorTree
                {
                    Root = new SerializedBehaviorNode()
                }
            );

            // When
            var loadDefaultActorBehaviorTreeHandler = new LoadDefaultActorBehaviorTreeHandler(
                mediatorMock.Object,
                serverInfoMock.Object,
                fileLoaderMock.Object,
                actorBehaviorTreeRepositoryMock.Object,
                actorBehaviorScriptRepositoryMock.Object
            );

            await loadDefaultActorBehaviorTreeHandler.Handle(
                new LoadDefaultActorBehaviorTree(),
                CancellationToken.None
            );

            // Then
            actorBehaviorTreeRepositoryMock.Verify(
                repository => repository.RegisterTree(
                    "DEFAULT",
                    It.IsAny<ActorBehaviorTreeShape>()
                )
            );
            mediatorMock.Verify(
                mediator => mediator.Send(
                    new BuildBehaviorScript(
                        "$DEFAULT$SCRIPT",
                        "return new BehaviorScriptResponse(BehaviorNodeStatus.SUCCESS);"
                    ),
                    CancellationToken.None
                )
            );
        }
    }
}