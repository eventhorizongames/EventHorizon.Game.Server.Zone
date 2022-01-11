namespace EventHorizon.Zone.System.ClientEntities.Tests.Delete;

using AutoFixture.Xunit2;

using EventHorizon.Test.Common.Attributes;
using EventHorizon.Zone.Core.Events.FileService;
using EventHorizon.Zone.Core.Model.FileService;
using EventHorizon.Zone.System.Backup.Events;
using EventHorizon.Zone.System.ClientEntities.Delete;
using EventHorizon.Zone.System.ClientEntities.Model;
using EventHorizon.Zone.System.ClientEntities.State;

using FluentAssertions;

using global::System.Collections.Concurrent;
using global::System.Collections.Generic;
using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

using Moq;

using Xunit;

public class DeleteClientEntityCommandHandlerTests
{
    [Theory, AutoMoqData]
    public async Task ShouldReturnSuccessOfTrueWhenNoErrorsAreFound(
        // Given
        string clientEntityId,
        string entityFileName,
        string entityFileFullName,
        string entityFileDirectoryName,
        string entityFileExtension,
        string entityFileContent,
        [Frozen] Mock<IMediator> mediatorMock,
        [Frozen] Mock<ClientEntityRepository> clientEntityRepositoryMock,
        DeleteClientEntityCommandHandler handler
    )
    {
        WhenSetupForSuccessfulDelete(
            clientEntityId,
            entityFileName,
            entityFileFullName,
            entityFileDirectoryName,
            entityFileExtension,
            entityFileContent,
            mediatorMock,
            clientEntityRepositoryMock
        );

        // When
        var actual = await handler.Handle(
            new DeleteClientEntityCommand(
                clientEntityId
            ),
            CancellationToken.None
        );

        // Then
        actual.Success
            .Should().BeTrue();
    }


    [Theory, AutoMoqData]
    public async Task ShouldBackupFileContentWhenFileIsFoundToDelete(
        // Given
        string clientEntityId,
        string entityFileName,
        string entityFileFullName,
        string entityFileDirectoryName,
        string entityFileExtension,
        string entityFileContent,
        [Frozen] Mock<IMediator> mediatorMock,
        [Frozen] Mock<ClientEntityRepository> clientEntityRepositoryMock,
        DeleteClientEntityCommandHandler handler
    )
    {
        WhenSetupForSuccessfulDelete(
            clientEntityId,
            entityFileName,
            entityFileFullName,
            entityFileDirectoryName,
            entityFileExtension,
            entityFileContent,
            mediatorMock,
            clientEntityRepositoryMock
        );

        // When
        var actual = await handler.Handle(
            new DeleteClientEntityCommand(
                clientEntityId
            ),
            CancellationToken.None
        );

        // Then
        mediatorMock.Verify(
            mock => mock.Send(
                It.Is<CreateBackupOfFileContentCommand>(
                    request => request.FileName == entityFileName
                        && request.FilePath.Contains("Client")
                        && request.FilePath.Contains("Entity")
                        && request.FileContent == entityFileContent
                ),
                CancellationToken.None
            )
        );
    }

    [Theory, AutoMoqData]
    public async Task ShouldDeleteFileWhenFileIsFoundToDelete(
        // Given
        string clientEntityId,
        string entityFileName,
        string entityFileFullName,
        string entityFileDirectoryName,
        string entityFileExtension,
        string entityFileContent,
        [Frozen] Mock<IMediator> mediatorMock,
        [Frozen] Mock<ClientEntityRepository> clientEntityRepositoryMock,
        DeleteClientEntityCommandHandler handler
    )
    {
        WhenSetupForSuccessfulDelete(
            clientEntityId,
            entityFileName,
            entityFileFullName,
            entityFileDirectoryName,
            entityFileExtension,
            entityFileContent,
            mediatorMock,
            clientEntityRepositoryMock
        );

        // When
        var actual = await handler.Handle(
            new DeleteClientEntityCommand(
                clientEntityId
            ),
            CancellationToken.None
        );

        // Then
        mediatorMock.Verify(
            mock => mock.Send(
                new DoesFileExist(
                    entityFileFullName
                ),
                CancellationToken.None
            )
        );
    }

    [Theory, AutoMoqData]
    public async Task ShouldReturnNotFoundErrorCodeWhenEntityIsNotInRepository(
        // Given
        string clientEntityId,
        [Frozen] Mock<ClientEntityRepository> clientEntityRepositoryMock,
        DeleteClientEntityCommandHandler handler
    )
    {
        var expected = ClientEntityErrorCodes.NOT_FOUND;

        clientEntityRepositoryMock.Setup(
            mock => mock.Find(
                clientEntityId
            )
        ).Returns(
            default(ClientEntity)
        );

        // When
        var actual = await handler.Handle(
            new DeleteClientEntityCommand(
                clientEntityId
            ),
            CancellationToken.None
        );

        // Then
        actual.Success
            .Should().BeFalse();
        actual.ErrorCode.Should().Be(
            expected
        );
    }

    [Theory, AutoMoqData]
    public async Task ShouldReturnFileNotFoundErrorCodeWhenEntityIsNotInRepository(
        // Given
        string clientEntityId,
        string entityFileName,
        string entityFileFullName,
        string entityFileDirectoryName,
        string entityFileExtension,
        string entityFileContent,
        [Frozen] Mock<IMediator> mediatorMock,
        [Frozen] Mock<ClientEntityRepository> clientEntityRepositoryMock,
        DeleteClientEntityCommandHandler handler
    )
    {
        var expected = ClientEntityErrorCodes.FILE_NOT_FOUND;
        WhenSetupForSuccessfulDelete(
            clientEntityId,
            entityFileName,
            entityFileFullName,
            entityFileDirectoryName,
            entityFileExtension,
            entityFileContent,
            mediatorMock,
            clientEntityRepositoryMock
        );

        mediatorMock.Setup(
            mock => mock.Send(
                new DoesFileExist(
                    entityFileFullName
                ),
                CancellationToken.None
            )
        ).ReturnsAsync(
            false
        );

        // When
        var actual = await handler.Handle(
            new DeleteClientEntityCommand(
                clientEntityId
            ),
            CancellationToken.None
        );

        // Then
        actual.Success.Should().BeFalse();
        actual.ErrorCode.Should().Be(
            expected
        );
    }

    [Theory, AutoMoqData]
    public async Task ShouldReturnFileNotFoundErrorCodeWhenEntityMetadataIsEmpty(
        // Given
        string clientEntityId,
        string entityFileName,
        string entityFileFullName,
        string entityFileDirectoryName,
        string entityFileExtension,
        string entityFileContent,
        [Frozen] Mock<IMediator> mediatorMock,
        [Frozen] Mock<ClientEntityRepository> clientEntityRepositoryMock,
        DeleteClientEntityCommandHandler handler
    )
    {
        var expected = ClientEntityErrorCodes.FILE_NOT_FOUND;
        WhenSetupForSuccessfulDelete(
            clientEntityId,
            entityFileName,
            entityFileFullName,
            entityFileDirectoryName,
            entityFileExtension,
            entityFileContent,
            mediatorMock,
            clientEntityRepositoryMock
        );

        clientEntityRepositoryMock.Setup(
            mock => mock.Find(
                clientEntityId
            )
        ).Returns(
            new ClientEntity(
                clientEntityId,
                new ConcurrentDictionary<string, object>()
            )
        );

        // When
        var actual = await handler.Handle(
            new DeleteClientEntityCommand(
                clientEntityId
            ),
            CancellationToken.None
        );

        // Then
        actual.Success.Should().BeFalse();
        actual.ErrorCode.Should().Be(
            expected
        );
    }

    private static void WhenSetupForSuccessfulDelete(
        string clientEntityId,
        string entityFileName,
        string entityFileFullName,
        string entityFileDirectoryName,
        string entityFileExtension,
        string entityFileContent,
        Mock<IMediator> mediatorMock,
        Mock<ClientEntityRepository> clientEntityRepositoryMock
    )
    {
        mediatorMock.Setup(
            mock => mock.Send(
                new GetFileInfo(
                    entityFileFullName
                ),
                CancellationToken.None
            )
        ).ReturnsAsync(
            new StandardFileInfo(
                entityFileName,
                entityFileDirectoryName,
                entityFileFullName,
                entityFileExtension
            )
        );

        mediatorMock.Setup(
            mock => mock.Send(
                new DoesFileExist(
                    entityFileFullName
                ),
                CancellationToken.None
            )
        ).ReturnsAsync(
            true
        );

        mediatorMock.Setup(
            mock => mock.Send(
                new ReadAllTextFromFile(
                    entityFileFullName
                ),
                CancellationToken.None
            )
        ).ReturnsAsync(
            entityFileContent
        );

        clientEntityRepositoryMock.Setup(
            mock => mock.Find(
                clientEntityId
            )
        ).Returns(
            new ClientEntity(
                clientEntityId,
                new ConcurrentDictionary<string, object>(
                    new Dictionary<string, object>
                    {
                            { ClientEntityConstants.METADATA_FILE_FULL_NAME, entityFileFullName },
                    }
                )
            )
        );
    }
}
