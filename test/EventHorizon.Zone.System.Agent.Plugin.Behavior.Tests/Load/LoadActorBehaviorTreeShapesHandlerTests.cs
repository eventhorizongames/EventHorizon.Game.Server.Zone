using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.FileService;
using EventHorizon.Zone.Core.Model.FileService;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.Core.Model.Json;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Api;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Load;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;
using EventHorizon.Zone.System.Server.Scripts.Events.Register;
using MediatR;
using Moq;
using Xunit;

namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Tests.Load
{
    public class LoadActorBehaviorTreeShapesHandlerTests
    {
        [Fact]
        public async Task ShouldAddTreesShapesFromDirectoryToRepository()
        {
            // Given
            var expectedTreeId = "Behaviors_BehaviorTreeToLoad.json";

            Func<StandardFileInfo, IDictionary<string, object>, Task> onProcessFile = null;
            IDictionary<string, object> arguments = null;
            var serverPath = Path.Combine(
                "root"
            );
            var behaviorsPath = Path.Combine(
                serverPath,
                "Behaviors"
            );
            var fileName = "BehaviorTreeToLoad.json";
            var fileFullName = Path.Combine(
                behaviorsPath,
                fileName
            );
            var fileExtension = ".exe";
            var fileInfo = new StandardFileInfo(
                fileName,
                behaviorsPath,
                fileFullName,
                fileExtension
            );
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
                mock => mock.ServerPath
            ).Returns(
                serverPath
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    It.IsAny<ProcessFilesRecursivelyFromDirectory>(),
                    CancellationToken.None
                )
            ).Callback<IRequest<Unit>, CancellationToken>(
                (evt, token) =>
                {
                    onProcessFile = ((ProcessFilesRecursivelyFromDirectory)evt).OnProcessFile;
                    arguments = ((ProcessFilesRecursivelyFromDirectory)evt).Arguments;
                }
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
            Assert.NotNull(
                onProcessFile
            );

            await onProcessFile(
                fileInfo,
                arguments
            );

            // Then
            actorBehaviorTreeRepositoryMock.Verify(
                repository => repository.RegisterTree(
                    expectedTreeId,
                    It.IsAny<ActorBehaviorTreeShape>()
                ),
                Times.Once()
            );
        }
    }
}