using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Events.FileService;
using EventHorizon.Zone.Core.Model.FileService;
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
            Func<StandardFileInfo, IDictionary<string, object>, Task> onProcessFile = null;
            IDictionary<string, object> arguments = null;
            var serverScriptsPath = Path.Combine(
                "root"
            );
            var behaviorPath = Path.Combine(
                serverScriptsPath,
                "Behaviors"
            );
            var fileName = "BehaviorTreeToLoad.json";
            var fileFullName = Path.Combine(
                behaviorPath,
                fileName
            );
            var fileExtension = ".exe";
            var fileContent = "file content";
            var fileInfo = new StandardFileInfo(
                fileName,
                behaviorPath,
                fileFullName,
                fileExtension
            );

            var expectedFileName = fileName;
            var expectedPath = "Behaviors";
            var expectedFileContent = fileContent;
            
            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();
            var systemProvidedAssemblyListMock = new Mock<SystemProvidedAssemblyList>();

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

            mediatorMock.Setup(
                mock => mock.Send(
                    It.IsAny<ReadAllTextFromFile>(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                fileContent
            );
            
            serverInfoMock.Setup(
                serverInfo => serverInfo.ServerScriptsPath
            ).Returns(
                serverScriptsPath
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

            await onProcessFile(
                fileInfo,
                arguments
            );

            // Then
            mediatorMock.Verify(
                mediator => mediator.Send(
                    It.Is<RegisterServerScriptCommand>(
                        command => 
                            command.FileName == expectedFileName
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