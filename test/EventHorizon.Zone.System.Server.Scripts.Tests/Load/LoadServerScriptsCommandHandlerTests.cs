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
            var onProcessFile = default(Func<StandardFileInfo, IDictionary<string, object>, Task>);
            var arguments = default(IDictionary<string, object>);
            var systemsOnProcessFile = default(Func<StandardFileInfo, IDictionary<string, object>, Task>);
            var systemsArguments = default(IDictionary<string, object>);

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
            var fileExtension = ".csx";
            var loadedScriptFileInfo = new StandardFileInfo(
                loadedScriptFileName,
                loadedScriptsPath,
                loadedScriptFileFullName,
                fileExtension
            );

            var systemsPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Systems_Data"
            );
            var systemsScriptsPath = Path.Combine(
                systemsPath,
                "Wizard",
                "Wizards"
            );
            var systemsScriptFileName = "Script.csx";
            var systemsScriptFileFullName = Path.Combine(
                systemsScriptsPath,
                systemsScriptFileName
            );
            var systemsScriptFileInfo = new StandardFileInfo(
                systemsScriptFileName,
                systemsScriptsPath,
                systemsScriptFileFullName,
                fileExtension
            );

            var expectedFileName = loadedScriptFileName.Replace(".csx", string.Empty);
            var expectedServerScript = new SetServerScriptDetailsCommand(
                new ServerScriptDetails(
                    expectedFileName,
                    "Loaded",
                    loadedScriptFileContent
                )
            );
            var systemsFileName = systemsScriptFileName.Replace(".csx", string.Empty);
            var systemsFilePath = Path.Combine(
                "Wizard",
                "Wizards"
            );
            var expectedSystemScript = new SetServerScriptDetailsCommand(
                new ServerScriptDetails(
                    systemsFileName,
                    systemsFilePath,
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

            serverInfoMock.Setup(
                mock => mock.SystemsPath
            ).Returns(
                systemsPath
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    It.IsAny<ProcessFilesRecursivelyFromDirectory>(),
                    CancellationToken.None
                )
            ).Callback<IRequest<Unit>, CancellationToken>(
                (evt, token) =>
                {
                    if (onProcessFile == null)
                    {
                        onProcessFile = ((ProcessFilesRecursivelyFromDirectory)evt).OnProcessFile;
                        arguments = ((ProcessFilesRecursivelyFromDirectory)evt).Arguments;
                    }
                    else
                    {
                        systemsOnProcessFile = ((ProcessFilesRecursivelyFromDirectory)evt).OnProcessFile;
                        systemsArguments = ((ProcessFilesRecursivelyFromDirectory)evt).Arguments;
                    }
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

            mediatorMock.Setup(
                mock => mock.Send(
                    new ReadAllTextFromFile(
                        systemsScriptFileFullName
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
            await systemsOnProcessFile(
                systemsScriptFileInfo,
                systemsArguments
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Send(
                    expectedServerScript,
                    CancellationToken.None
                )
            );
            mediatorMock.Verify(
                mock => mock.Send(
                    expectedSystemScript,
                    CancellationToken.None
                )
            );
            detailsRepositoryMock.Verify(
                mock => mock.Clear()
            );
        }

        [Fact]
        public async Task ShouldNotSendSetCommandWhenExtensionIsNotCSX()
        {
            // Given
            var onProcessFile = default(Func<StandardFileInfo, IDictionary<string, object>, Task>);
            var arguments = default(IDictionary<string, object>);
            var systemOnProcessFile = default(Func<StandardFileInfo, IDictionary<string, object>, Task>);
            var systemArguments = default(IDictionary<string, object>);

            var serverScriptsPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Scripts"
            );
            var loadedScriptsPath = Path.Combine(
                serverScriptsPath,
                "Loaded"
            );
            var loadedScriptFileName = "Script.exe";
            var loadedScriptFileFullName = Path.Combine(
                loadedScriptsPath,
                loadedScriptFileName
            );
            var fileExtension = ".exe";
            var loadedScriptFileInfo = new StandardFileInfo(
                loadedScriptFileName,
                loadedScriptsPath,
                loadedScriptFileFullName,
                fileExtension
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
                    if (onProcessFile == null)
                    {
                        onProcessFile = ((ProcessFilesRecursivelyFromDirectory)evt).OnProcessFile;
                        arguments = ((ProcessFilesRecursivelyFromDirectory)evt).Arguments;
                    }
                    else
                    {
                        systemOnProcessFile = ((ProcessFilesRecursivelyFromDirectory)evt).OnProcessFile;
                        systemArguments = ((ProcessFilesRecursivelyFromDirectory)evt).Arguments;
                    }
                }
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
                    It.IsAny<SetServerScriptDetailsCommand>(),
                    CancellationToken.None
                ),
                Times.Never()
            );
        }
    }
}
