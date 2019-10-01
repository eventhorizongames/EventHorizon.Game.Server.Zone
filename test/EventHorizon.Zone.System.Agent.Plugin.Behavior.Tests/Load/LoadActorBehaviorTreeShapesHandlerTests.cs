using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.Core.Model.Json;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Api;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Load;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;
using MediatR;
using Moq;
using Xunit;
using static EventHorizon.Zone.System.Agent.Plugin.Behavior.Load.LoadActorBehaviorScripts;
using static EventHorizon.Zone.System.Agent.Plugin.Behavior.Load.LoadActorBehaviorTreeShapes;

namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Tests.Load
{
    public class LoadActorBehaviorTreeShapesHandlerTests
    {
        [Fact]
        public async Task ShouldAddTreesShapesFromDirectoryToRepository()
        {
            // Given
            var expectedTreeId1 = "Behaviors_Sub_SubBehaviorTreeToLoad.json";
            var expectedTreeId2 = "Behaviors_BehaviorTreeToLoad.json";
            var serializedAgentBehaviorTree = new SerializedAgentBehaviorTree
            {
                Root = new SerializedBehaviorNode
                {
                    NodeList = new List<SerializedBehaviorNode>()
                }
            };

            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();
            var jsonFileLoaderMock = new Mock<IJsonFileLoader>();
            var actorBehaviorTreeRepositoryMock = new Mock<ActorBehaviorTreeRepository>();

            serverInfoMock.Setup(
                serverInfo => serverInfo.ServerPath
            ).Returns(
                Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "Load"
                )
            );
            jsonFileLoaderMock.Setup(
                loader => loader.GetFile<SerializedAgentBehaviorTree>(
                    It.IsAny<string>()
                )
            ).ReturnsAsync(
                serializedAgentBehaviorTree
            );

            // When
            var loadServerModuleSystemHandler = new LoadActorBehaviorTreeShapesHandler(
                mediatorMock.Object,
                serverInfoMock.Object,
                jsonFileLoaderMock.Object,
                actorBehaviorTreeRepositoryMock.Object
            );

            await loadServerModuleSystemHandler.Handle(
                new LoadActorBehaviorTreeShapes(),
                CancellationToken.None
            );

            // Then
            actorBehaviorTreeRepositoryMock.Verify(
                repository => repository.RegisterTree(
                    expectedTreeId1,
                    It.IsAny<ActorBehaviorTreeShape>()
                ),
                Times.Once()
            );
            actorBehaviorTreeRepositoryMock.Verify(
                repository => repository.RegisterTree(
                    expectedTreeId2,
                    It.IsAny<ActorBehaviorTreeShape>()
                ),
                Times.Once()
            );
        }
        [Fact]
        public async Task ShouldClearRepositoryOnHandle()
        {
            // Given
            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();
            var jsonFileLoaderMock = new Mock<IJsonFileLoader>();
            var agentBehaviorScriptRepositoryMock = new Mock<ActorBehaviorScriptRepository>();

            serverInfoMock.Setup(
                serverInfo => serverInfo.ServerPath
            ).Returns(
                Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "Load"
                )
            );

            // When
            var loadServerModuleSystemHandler = new LoadActorBehaviorScriptsHandler(
                mediatorMock.Object,
                serverInfoMock.Object,
                jsonFileLoaderMock.Object,
                agentBehaviorScriptRepositoryMock.Object
            );

            await loadServerModuleSystemHandler.Handle(
                new LoadActorBehaviorScripts(),
                CancellationToken.None
            );

            // Then
            agentBehaviorScriptRepositoryMock.Verify(
                repository => repository.Clear(),
                Times.Once()
            );
        }
    }
}