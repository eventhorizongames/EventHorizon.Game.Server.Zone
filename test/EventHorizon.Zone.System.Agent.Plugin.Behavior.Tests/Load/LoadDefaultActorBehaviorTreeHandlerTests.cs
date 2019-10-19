using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
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
    public class LoadDefaultActorBehaviorTreeHandlerTests
    {
        [Fact]
        public async Task ShouldRegisterDefaultBehaviorTreeShapeWhenCalled()
        {
            // Given
            var systemPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Load",
                "System"
            );
            var defaultBehaviorShapePath = Path.Combine(
                systemPath,
                "Behaviors",
                "$DEFAULT$SHAPE.json"
            );

            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();
            var fileLoaderMock = new Mock<IJsonFileLoader>();
            var actorBehaviorTreeRepositoryMock = new Mock<ActorBehaviorTreeRepository>();
            var systemProvidedAssemblyListMock = new Mock<SystemProvidedAssemblyList>();

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
                        command => command.FileName == "$DEFAULT$SCRIPT"
                                &&
                                command.Path == string.Empty
                                &&
                                command.ScriptString == "return new BehaviorScriptResponse(BehaviorNodeStatus.SUCCESS);"
                    ),
                    CancellationToken.None
                )
            );
        }
    }
}