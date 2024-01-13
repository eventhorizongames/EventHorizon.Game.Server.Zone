namespace EventHorizon.Zone.System.Wizard.Tests.Load;

using global::System;
using EventHorizon.Zone.Core.Events.FileService;
using EventHorizon.Zone.Core.Model.FileService;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.Core.Model.Json;
using EventHorizon.Zone.System.Wizard.Api;
using EventHorizon.Zone.System.Wizard.Load;
using EventHorizon.Zone.System.Wizard.Model;
using FluentAssertions;
using global::System.Collections.Generic;
using global::System.IO;
using global::System.Threading;
using global::System.Threading.Tasks;
using MediatR;
using Moq;
using Xunit;

public class LoadWizardListCommandHandlerTests
{
    [Fact]
    public async Task ShouldHaveWizardsServerPathWhenProcessingFilesRecursively()
    {
        // Given
        var serverPath = "server-path";

        var fromDirectory = default(string);
        var onProcessFile = default(Func<StandardFileInfo, IDictionary<string, object>, Task>);
        var arguments = default(IDictionary<string, object>);

        var expected = Path.Combine(serverPath, "Wizards");

        var mediatorMock = new Mock<IMediator>();
        var serverInfoMock = new Mock<ServerInfo>();
        var jsonFileLoaderMock = new Mock<IJsonFileLoader>();
        var wizardRepositoryMock = new Mock<WizardRepository>();

        mediatorMock
            .Setup(
                mock =>
                    mock.Send(
                        It.IsAny<ProcessFilesRecursivelyFromDirectory>(),
                        CancellationToken.None
                    )
            )
            .Callback<IRequest, CancellationToken>(
                (evt, token) =>
                {
                    fromDirectory = ((ProcessFilesRecursivelyFromDirectory)evt).FromDirectory;
                    onProcessFile = ((ProcessFilesRecursivelyFromDirectory)evt).OnProcessFile;
                    arguments = ((ProcessFilesRecursivelyFromDirectory)evt).Arguments;
                }
            );

        serverInfoMock.Setup(mock => mock.ServerPath).Returns(serverPath);

        // When
        var handler = new LoadWizardListCommandHandler(
            mediatorMock.Object,
            serverInfoMock.Object,
            jsonFileLoaderMock.Object,
            wizardRepositoryMock.Object
        );
        var actual = await handler.Handle(new LoadWizardListCommand(), CancellationToken.None);

        // Then
        fromDirectory.Should().NotBeNull().And.Subject.Should().Be(expected);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("     ")]
    public async Task ShouldNotAddLoadedWizardToRepositoryWhenIdIsInvalid(string wizardId)
    {
        // Given
        var serverPath = "server-path";
        var fileFullName = "file-full-name";
        var fileInfo = new StandardFileInfo("name", "director-name", fileFullName, "ext");
        var wizard = new WizardMetadata { Id = wizardId, };
        var arguments = new Dictionary<string, object>();

        var actualFromDirectory = default(string);
        var actualOnProcessFile = default(Func<
            StandardFileInfo,
            IDictionary<string, object>,
            Task
        >);
        var actualArguments = default(IDictionary<string, object>);

        var mediatorMock = new Mock<IMediator>();
        var serverInfoMock = new Mock<ServerInfo>();
        var jsonFileLoaderMock = new Mock<IJsonFileLoader>();
        var wizardRepositoryMock = new Mock<WizardRepository>();

        mediatorMock
            .Setup(
                mock =>
                    mock.Send(
                        It.IsAny<ProcessFilesRecursivelyFromDirectory>(),
                        CancellationToken.None
                    )
            )
            .Callback<IRequest, CancellationToken>(
                (evt, token) =>
                {
                    actualFromDirectory = (
                        (ProcessFilesRecursivelyFromDirectory)evt
                    ).FromDirectory;
                    actualOnProcessFile = (
                        (ProcessFilesRecursivelyFromDirectory)evt
                    ).OnProcessFile;
                    actualArguments = ((ProcessFilesRecursivelyFromDirectory)evt).Arguments;
                }
            );

        serverInfoMock.Setup(mock => mock.ServerPath).Returns(serverPath);

        jsonFileLoaderMock
            .Setup(mock => mock.GetFile<WizardMetadata>(fileFullName))
            .ReturnsAsync(wizard);

        // When
        var handler = new LoadWizardListCommandHandler(
            mediatorMock.Object,
            serverInfoMock.Object,
            jsonFileLoaderMock.Object,
            wizardRepositoryMock.Object
        );
        await handler.Handle(new LoadWizardListCommand(), CancellationToken.None);

        actualOnProcessFile.Should().NotBeNull();
        await actualOnProcessFile(fileInfo, arguments);

        // Then
        jsonFileLoaderMock.Verify(mock => mock.GetFile<WizardMetadata>(fileFullName));

        wizardRepositoryMock.Verify(
            mock => mock.Set(It.IsAny<string>(), It.IsAny<WizardMetadata>()),
            Times.Never()
        );
    }

    [Fact]
    public async Task ShouldAddLoadedWizardToRepositoryWhenWhenIdIsValid()
    {
        // Given
        var wizardId = "is-valid-id";
        var serverPath = "server-path";
        var fileFullName = "file-full-name";
        var fileInfo = new StandardFileInfo("name", "director-name", fileFullName, "ext");
        var wizard = new WizardMetadata { Id = wizardId, };
        var arguments = new Dictionary<string, object>();

        var actualFromDirectory = default(string);
        var actualOnProcessFile = default(Func<
            StandardFileInfo,
            IDictionary<string, object>,
            Task
        >);
        var actualArguments = default(IDictionary<string, object>);

        var mediatorMock = new Mock<IMediator>();
        var serverInfoMock = new Mock<ServerInfo>();
        var jsonFileLoaderMock = new Mock<IJsonFileLoader>();
        var wizardRepositoryMock = new Mock<WizardRepository>();

        mediatorMock
            .Setup(
                mock =>
                    mock.Send(
                        It.IsAny<ProcessFilesRecursivelyFromDirectory>(),
                        CancellationToken.None
                    )
            )
            .Callback<IRequest, CancellationToken>(
                (evt, token) =>
                {
                    actualFromDirectory = (
                        (ProcessFilesRecursivelyFromDirectory)evt
                    ).FromDirectory;
                    actualOnProcessFile = (
                        (ProcessFilesRecursivelyFromDirectory)evt
                    ).OnProcessFile;
                    actualArguments = ((ProcessFilesRecursivelyFromDirectory)evt).Arguments;
                }
            );

        serverInfoMock.Setup(mock => mock.ServerPath).Returns(serverPath);

        jsonFileLoaderMock
            .Setup(mock => mock.GetFile<WizardMetadata>(fileFullName))
            .ReturnsAsync(wizard);

        // When
        var handler = new LoadWizardListCommandHandler(
            mediatorMock.Object,
            serverInfoMock.Object,
            jsonFileLoaderMock.Object,
            wizardRepositoryMock.Object
        );
        await handler.Handle(new LoadWizardListCommand(), CancellationToken.None);

        actualOnProcessFile.Should().NotBeNull();
        await actualOnProcessFile(fileInfo, arguments);

        // Then
        jsonFileLoaderMock.Verify(mock => mock.GetFile<WizardMetadata>(fileFullName));

        wizardRepositoryMock.Verify(mock => mock.Set(wizardId, wizard));
    }
}
