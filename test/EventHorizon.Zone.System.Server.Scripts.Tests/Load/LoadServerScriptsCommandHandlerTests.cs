namespace EventHorizon.Zone.System.Server.Scripts.Tests.Load
{
    using global::System;
    using global::System.Collections.Generic;
    using global::System.IO;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using EventHorizon.Zone.Core.Events.FileService;
    using EventHorizon.Zone.Core.Model.FileService;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.System.Server.Scripts.Events.Load;
    using EventHorizon.Zone.System.Server.Scripts.Events.Register;
    using MediatR;
    using Moq;
    using Xunit;
    using EventHorizon.Zone.System.Server.Scripts.Load;

    public class LoadServerScriptsCommandHandlerTests
    {
        [Fact]
        public async Task TestName()
        {
            // Given
            Func<StandardFileInfo, IDictionary<string, object>, Task> onProcessFile = null;
            IDictionary<string, object> arguments = null;
            var serverScriptsPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Scripts"
            );
            var loadedScriptsPath = Path.Combine(
                serverScriptsPath,
                "Loaded"
            );
            var loadedScriptFileName = "Script.csx";
            var loadedScriptFileFullName = Path.Combine(
                loadedScriptsPath,
                loadedScriptFileName
            );
            var loadedScriptFileContent = "// Script Comment";
            var fileExtension = ".exe";
            var loadedScriptFileInfo = new StandardFileInfo(
                loadedScriptFileName,
                loadedScriptsPath,
                loadedScriptFileFullName,
                fileExtension
            );

            var expectedFileName = loadedScriptFileName;
            var expectedPath = "Loaded";
            var expectedFileContent = loadedScriptFileContent;

            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();
            var systemProvidedAssemblyListMock = new Mock<SystemProvidedAssemblyList>();

            serverInfoMock.Setup(
                mock => mock.ServerScriptsPath
            ).Returns(
                serverScriptsPath
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

            mediatorMock.Setup(
                mock => mock.Send(
                    new ReadAllTextFromFile(
                        loadedScriptFileFullName
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                loadedScriptFileContent
            );

            // When
            var handler = new LoadServerScriptsCommandHandler(
                mediatorMock.Object,
                serverInfoMock.Object,
                systemProvidedAssemblyListMock.Object
            );

            await handler.Handle(
                new LoadServerScriptsCommand(),
                CancellationToken.None
            );
            Assert.NotNull(
                onProcessFile
            );

            await onProcessFile(
                loadedScriptFileInfo,
                arguments
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Send(
                    It.Is<RegisterServerScriptCommand>(
                        command => command.FileName == expectedFileName
                            && command.Path == expectedPath
                            && command.ScriptString == expectedFileContent
                    ),
                    CancellationToken.None
                )
            );
        }
    }
}