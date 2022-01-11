namespace EventHorizon.Zone.System.ClientEntities.Tests.Save;

using AutoFixture.Xunit2;

using EventHorizon.Test.Common.Attributes;
using EventHorizon.Zone.Core.Events.FileService;
using EventHorizon.Zone.Core.Model.FileService;
using EventHorizon.Zone.System.Backup.Events;
using EventHorizon.Zone.System.ClientEntities.Model;
using EventHorizon.Zone.System.ClientEntities.Save;
using EventHorizon.Zone.System.ClientEntities.State;

using FluentAssertions;

using global::System;
using global::System.Collections.Concurrent;
using global::System.Collections.Generic;
using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

using Moq;

using Xunit;

public class SaveClientEntityCommandHandlerTests
{
    [Theory, AutoMoqData]
    public async Task ShouldReturnNewlyRegisteredEntityWhenSuccessfulySaved(
        // Given
        string clientEntityId,
        string fileName,
        string fileDirectoryName,
        string fileFullName,
        string fileExtension,
        string fileContent,
        [Frozen] Mock<ISender> senderMock,
        [Frozen] Mock<ClientEntityRepository> repositoryMock,
        SaveClientEntityCommandHandler handler
    )
    {
        var (clientEntity, _) = WhenSetupForSuccessfulSave(
            clientEntityId,
            fileName,
            fileDirectoryName,
            fileFullName,
            fileExtension,
            fileContent,
            senderMock,
            repositoryMock
        );
        var expected = clientEntity;

        // When
        var actual = await handler.Handle(
            new SaveClientEntityCommand(
                clientEntity
            ),
            CancellationToken.None
        );

        // Then
        actual.Success.Should().BeTrue();
        actual.ClientEntity
            .Should().Be(expected);
    }

    [Theory, AutoMoqData]
    public async Task ShouldCreateBackupWhenFileInfoExistsForFullName(
        // Given
        string clientEntityId,
        string fileName,
        string fileDirectoryName,
        string fileFullName,
        string fileExtension,
        string fileContent,
        [Frozen] Mock<ISender> senderMock,
        [Frozen] Mock<ClientEntityRepository> repositoryMock,
        SaveClientEntityCommandHandler handler
    )
    {
        var (clientEntity, fileInfo) = WhenSetupForSuccessfulSave(
            clientEntityId,
            fileName,
            fileDirectoryName,
            fileFullName,
            fileExtension,
            fileContent,
            senderMock,
            repositoryMock
        );

        var expectedPath = new string[] { "Client", "Entity" };
        var expectedFileName = fileName;
        var expectedFileContent = fileContent;

        // When
        await handler.Handle(
            new SaveClientEntityCommand(
                clientEntity
            ),
            CancellationToken.None
        );

        // Then
        senderMock.Verify(
            mock => mock.Send(
                It.Is<CreateBackupOfFileContentCommand>(
                    actual => actual.FilePath.Contains(expectedPath[0])
                        && actual.FilePath.Contains(expectedPath[1])
                        && actual.FileName == expectedFileName
                        && actual.FileContent == expectedFileContent
                ),
                CancellationToken.None
            )
        );
    }

    [Theory, AutoMoqData]
    public async Task ReturnExceptionErrorCodeResponseWhenExceptionOnBackup(
        // Given
        string clientEntityId,
        string fileName,
        string fileDirectoryName,
        string fileFullName,
        string fileExtension,
        string fileContent,
        [Frozen] Mock<ISender> senderMock,
        [Frozen] Mock<ClientEntityRepository> repositoryMock,
        SaveClientEntityCommandHandler handler
    )
    {
        var expected = "exception";
        var (clientEntity, _) = WhenSetupForSuccessfulSave(
            clientEntityId,
            fileName,
            fileDirectoryName,
            fileFullName,
            fileExtension,
            fileContent,
            senderMock,
            repositoryMock
        );

        senderMock.Setup(
            mock => mock.Send(
                It.IsAny<CreateBackupOfFileContentCommand>(),
                CancellationToken.None
            )
        ).Throws(
            new Exception("Failed to Backup File.")
        );

        // When
        var actual = await handler.Handle(
            new SaveClientEntityCommand(
                clientEntity
            ),
            CancellationToken.None
        );

        // Then
        actual.Success
            .Should().BeFalse();
        actual.ErrorCode.Should().Be(expected);
    }

    [Theory, AutoMoqData]
    public async Task DoesNotCauseFailureWhenFileFullNameIsMissingFromRawData(
        // Given
        string clientEntityId,
        string fileName,
        string fileDirectoryName,
        string fileFullName,
        string fileExtension,
        string fileContent,
        [Frozen] Mock<ISender> senderMock,
        [Frozen] Mock<ClientEntityRepository> repositoryMock,
        SaveClientEntityCommandHandler handler
    )
    {
        var (clientEntity, _) = WhenSetupForSuccessfulSave(
            clientEntityId,
            fileName,
            fileDirectoryName,
            fileFullName,
            fileExtension,
            fileContent,
            senderMock,
            repositoryMock
        );
        clientEntity.RawData.Remove(
            ClientEntityConstants.METADATA_FILE_FULL_NAME,
            out _
        );

        // When
        var actual = await handler.Handle(
            new SaveClientEntityCommand(
                clientEntity
            ),
            CancellationToken.None
        );

        // Then
        actual.Success
            .Should().BeTrue();
    }

    private static (ClientEntity, StandardFileInfo) WhenSetupForSuccessfulSave(
        string clientEntityId,
        string fileName,
        string fileDirectoryName,
        string fileFullName,
        string fileExtension,
        string fileContent,
        Mock<ISender> senderMock,
        Mock<ClientEntityRepository> repositoryMock
    )
    {
        var clientEntity = new ClientEntity(
            clientEntityId,
            new ConcurrentDictionary<string, object>(
                new Dictionary<string, object>
                {
                    [ClientEntityConstants.METADATA_FILE_FULL_NAME] = fileFullName,
                }
            )
        );

        var fileInfo = new StandardFileInfo(
            fileName,
            fileDirectoryName,
            fileFullName,
            fileExtension
        );

        senderMock.Setup(
            mock => mock.Send(
                new GetFileInfo(
                    fileFullName
                ),
                CancellationToken.None
            )
        ).ReturnsAsync(
            fileInfo
        );

        senderMock.Setup(
            mock => mock.Send(
                new DoesFileExist(
                    fileFullName
                ),
                CancellationToken.None
            )
        ).ReturnsAsync(
            true
        );

        senderMock.Setup(
            mock => mock.Send(
                new ReadAllTextFromFile(
                    fileFullName
                ),
                CancellationToken.None
            )
        ).ReturnsAsync(
            fileContent
        );

        repositoryMock.Setup(
            mock => mock.Find(
                clientEntityId
            )
        ).Returns(
            clientEntity
        );

        return (clientEntity, fileInfo);
    }
}
