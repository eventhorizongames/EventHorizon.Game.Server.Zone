using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.Core.Model.Json;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Api;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Load;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Script;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Script.Builder;
using MediatR;
using Moq;
using Xunit;
using static EventHorizon.Zone.System.Agent.Plugin.Behavior.Load.LoadActorBehaviorScripts;

namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Tests.Load
{
    public class LoadActorBehaviorScriptsHandlerTests
    {
        [Fact]
        public async Task ShouldAddScriptsFromFromDirectoryToRepository()
        {
            // Given
            var expectedBuildBehaviorScript1 = new BuildBehaviorScript(
                "Behavior_ScriptToLoad.csx",
                "throw new NotImplementedException(\"ScriptToLoad Not Implemented\");"
            );
            var expectedBuildBehaviorScript2 = new BuildBehaviorScript(
                "Behavior_Sub_SubScriptToLoad.csx",
                "throw new NotImplementedException(\"SubScriptToLoad Not Implemented\");"
            );
            var expectedBehaviorScript = new BehaviorScript();
            var expectedAddedToRepository = 2;

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
            mediatorMock.Verify(
                mediator => mediator.Send(
                    expectedBuildBehaviorScript1,
                    CancellationToken.None
                )
            );
            mediatorMock.Verify(
                mediator => mediator.Send(
                    expectedBuildBehaviorScript2,
                    CancellationToken.None
                )
            );
            agentBehaviorScriptRepositoryMock.Verify(
                repository => repository.Add(
                    expectedBehaviorScript
                ),
                Times.Exactly(
                    expectedAddedToRepository
                )
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