namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Tests.Load
{
    using global::System;
    using global::System.Collections.Generic;
    using global::System.IO;
    using global::System.Reflection;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using EventHorizon.Zone.Core.Events.FileService;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.Core.Model.Json;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.Api;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.Load;
    using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;
    using EventHorizon.Zone.System.Server.Scripts.Events.Register;
    using MediatR;
    using Moq;
    using Xunit;

    public class LoadDefaultActorBehaviorTreeHandlerTests
    {
        [Fact]
        public async Task ShouldRegisterDefaultBehaviorTreeShapeWhenCalled()
        {
            // Given
            var expectedName = "$DEFAULT$SCRIPT";
            var fileName = $"{expectedName}.csx";
            var expectedPath = "";
            var expectedFileContent = "file-content";
            var serverScriptsPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Load"
            );
            var systemPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "System"
            );
            var defaultBehaviorShapePath = Path.Combine(
                systemPath,
                "Behaviors",
                "$DEFAULT$SHAPE.json"
            );
            var defaultBehaviorScriptPath = Path.Combine(
                serverScriptsPath,
                "System",
                "Behaviors",
                fileName
            );

            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();
            var fileLoaderMock = new Mock<IJsonFileLoader>();
            var actorBehaviorTreeRepositoryMock = new Mock<ActorBehaviorTreeRepository>();
            var systemProvidedAssemblyListMock = new Mock<SystemProvidedAssemblyList>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new ReadAllTextFromFile(
                        defaultBehaviorScriptPath
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                expectedFileContent
            );

            serverInfoMock.SetupGet(
                mock => mock.ServerScriptsPath
            ).Returns(
                serverScriptsPath
            );
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

            systemProvidedAssemblyListMock.Setup(
                mock => mock.List
            ).Returns(
                new List<Assembly>()
            );

            // When
            var loadDefaultActorBehaviorTreeHandler = new LoadDefaultActorBehaviorTreeHandler(
                mediatorMock.Object,
                serverInfoMock.Object,
                fileLoaderMock.Object,
                actorBehaviorTreeRepositoryMock.Object,
                systemProvidedAssemblyListMock.Object
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
                    It.Is<RegisterServerScriptCommand>(
                        command => 
                            command.FileName == expectedName
                            &&
                            command.Path == expectedPath
                            &&
                            command.ScriptString == expectedFileContent
                    ),
                    CancellationToken.None
                )
            );
        }
    }
}