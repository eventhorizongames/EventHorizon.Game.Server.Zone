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
    using MediatR;
    using Moq;
    using Xunit;
    using EventHorizon.Zone.System.Server.Scripts.Load;
    using EventHorizon.Zone.System.Server.Scripts.Set;
    using EventHorizon.Zone.System.Server.Scripts.Model.Details;
    using EventHorizon.Zone.System.Server.Scripts.State;
    using FluentAssertions;

    public class LoadServerScriptsCommandHandlerTests
    {
        [Fact]
        public async Task ShouldSendSetEventWhenOnProcessFileIsRanForFoundFileInfo()
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

            var expectedFileName = loadedScriptFileName.Replace(".csx", string.Empty);
            var expected = new SetServerScriptDetailsCommand(
                new ServerScriptDetails(
                    expectedFileName,
                    "Loaded",
                    loadedScriptFileContent
                )
            );

            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();
            var detailsRepositoryMock = new Mock<ServerScriptDetailsRepository>();

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
                detailsRepositoryMock.Object
            );

            await handler.Handle(
                new LoadServerScriptsCommand(),
                CancellationToken.None
            );
            onProcessFile.Should().NotBeNull();

            await onProcessFile(
                loadedScriptFileInfo,
                arguments
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Send(
                    expected,
                    CancellationToken.None
                )
            );
            detailsRepositoryMock.Verify(
                mock => mock.Clear()
            );
        }
    }
}
