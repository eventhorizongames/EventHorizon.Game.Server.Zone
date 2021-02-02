namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Tests.Load
{
    using global::System;
    using global::System.IO;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.Core.Model.Json;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.Api;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.Load;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;
    using Moq;
    using Xunit;

    public class LoadDefaultActorBehaviorTreeHandlerTests
    {
        [Fact]
        public async Task ShouldRegisterDefaultBehaviorTreeShapeWhenCalled()
        {
            // Given
            var systemPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "System"
            );
            var defaultBehaviorShapePath = Path.Combine(
                systemPath,
                "Behaviors",
                "$DEFAULT$SHAPE.json"
            );

            var serverInfoMock = new Mock<ServerInfo>();
            var fileLoaderMock = new Mock<IJsonFileLoader>();
            var actorBehaviorTreeRepositoryMock = new Mock<ActorBehaviorTreeRepository>();

            serverInfoMock.SetupGet(
                mock => mock.SystemPath
            ).Returns(
                systemPath
            );

            fileLoaderMock.Setup(
                mock => mock.GetFile<SerializedAgentBehaviorTree>(
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
                serverInfoMock.Object,
                fileLoaderMock.Object,
                actorBehaviorTreeRepositoryMock.Object
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
        }
    }
}