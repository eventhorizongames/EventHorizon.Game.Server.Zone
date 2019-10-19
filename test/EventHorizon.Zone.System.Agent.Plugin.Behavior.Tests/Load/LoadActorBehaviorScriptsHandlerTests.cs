using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.System.Agent.Plugin.Behavior.Load;
using EventHorizon.Zone.System.Server.Scripts.Events.Register;
using MediatR;
using Moq;
using Xunit;

namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Tests.Load
{
    public class LoadActorBehaviorScriptsHandlerTests
    {
        [Fact]
        public async Task ShouldAddScriptsFromFromDirectoryToRepository()
        {
            // Given            
            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();
            var systemProvidedAssemblyListMock = new Mock<SystemProvidedAssemblyList>();

            serverInfoMock.Setup(
                serverInfo => serverInfo.ServerPath
            ).Returns(
                Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "Load"
                )
            );

            systemProvidedAssemblyListMock.Setup(
                mock => mock.List
            ).Returns(
                new List<Assembly>()
            );

            // When
            var loadServerModuleSystemHandler = new LoadActorBehaviorScriptsHandler(
                mediatorMock.Object,
                serverInfoMock.Object,
                systemProvidedAssemblyListMock.Object
            );

            await loadServerModuleSystemHandler.Handle(
                new LoadActorBehaviorScripts(),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mediator => mediator.Send(
                    It.Is<RegisterServerScriptCommand>(
                        command => 
                            command.FileName == "ScriptToLoad.csx"
                            &&
                            command.Path == "Behavior"
                            &&
                            command.ScriptString == "throw new NotImplementedException(\"ScriptToLoad Not Implemented\");"
                    ),
                    CancellationToken.None
                )
            );
            mediatorMock.Verify(
                mediator => mediator.Send(
                    It.Is<RegisterServerScriptCommand>(
                        command => 
                            command.FileName == "SubScriptToLoad.csx"
                            &&
                            command.Path == Path.Combine("Behavior", "Sub")
                            &&
                            command.ScriptString == "throw new NotImplementedException(\"SubScriptToLoad Not Implemented\");"
                    ),
                    CancellationToken.None
                )
            );
        }
    }
}