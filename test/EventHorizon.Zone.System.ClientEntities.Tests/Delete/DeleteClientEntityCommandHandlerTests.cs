namespace EventHorizon.Zone.System.ClientEntities.Tests.Delete
{
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
        [Fact]
        public async Task ShouldReturnSuccessOfTrueWhenNoErrorsAreFound()
        {
            // Given
            var clientEntityId = "client-entity-id";
            var entityFileName = "entity-file-name";
            var entityFileFullName = "entity-file-full-name";
            var entityFileDirectoryName = "entity-file-directory-name";
            var entityFileExtension = "entity-file-extension";
            var clientEntity = new ClientEntity(
                clientEntityId,
                new ConcurrentDictionary<string, object>(
                    new Dictionary<string, object>
                    {
                        { ClientEntityConstants.METADATA_FILE_FULL_NAME, entityFileFullName },
                    }
                )
            );

            var mediatorMock = new Mock<IMediator>();
            var clientEntityRepositoryMock = new Mock<ClientEntityRepository>();

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

            clientEntityRepositoryMock.Setup(
                mock => mock.Find(
                    clientEntityId
                )
            ).Returns(
                clientEntity
            );

            // When
            var handler = new DeleteClientEntityCommandHandler(
                mediatorMock.Object,
                clientEntityRepositoryMock.Object
            );
            var actual = await handler.Handle(
                new DeleteClientEntityCommand(
                    clientEntityId
                ),
                CancellationToken.None
            );

            // Then
            actual.Success.Should().BeTrue();
        }

        [Fact]
        public async Task ShouldBackupFileContentWhenFileIsFoundToDelete()
        {
            // Given
            var clientEntityId = "client-entity-id";
            var entityFileName = "entity-file-name";
            var entityFileFullName = "entity-file-full-name";
            var entityFileDirectoryName = "entity-file-directory-name";
            var entityFileExtension = "entity-file-extension";
            var entityFileContent = "entity-file-content";
            var clientEntity = new ClientEntity(
                clientEntityId,
                new ConcurrentDictionary<string, object>(
                    new Dictionary<string, object>
                    {
                        { ClientEntityConstants.METADATA_FILE_FULL_NAME, entityFileFullName },
                    }
                )
            );

            var mediatorMock = new Mock<IMediator>();
            var clientEntityRepositoryMock = new Mock<ClientEntityRepository>();

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
                clientEntity
            );

            // When
            var handler = new DeleteClientEntityCommandHandler(
                mediatorMock.Object,
                clientEntityRepositoryMock.Object
            );
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

        [Fact]
        public async Task ShouldDeleteFileWhenFileIsFoundToDelete()
        {
            // Given
            var clientEntityId = "client-entity-id";
            var entityFileName = "entity-file-name";
            var entityFileFullName = "entity-file-full-name";
            var entityFileDirectoryName = "entity-file-directory-name";
            var entityFileExtension = "entity-file-extension";
            var clientEntity = new ClientEntity(
                clientEntityId,
                new ConcurrentDictionary<string, object>(
                    new Dictionary<string, object>
                    {
                        { ClientEntityConstants.METADATA_FILE_FULL_NAME, entityFileFullName },
                    }
                )
            );

            var mediatorMock = new Mock<IMediator>();
            var clientEntityRepositoryMock = new Mock<ClientEntityRepository>();

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

            clientEntityRepositoryMock.Setup(
                mock => mock.Find(
                    clientEntityId
                )
            ).Returns(
                clientEntity
            );

            // When
            var handler = new DeleteClientEntityCommandHandler(
                mediatorMock.Object,
                clientEntityRepositoryMock.Object
            );
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
        
        [Fact]
        public async Task ShouldReturnNotFoundErrorCodeWhenEntityIsNotInRepository()
        {
            // Given
            var expected = ClientEntityErrorCodes.NOT_FOUND;
            var clientEntityId = "client-entity-id";

            var mediatorMock = new Mock<IMediator>();
            var clientEntityRepositoryMock = new Mock<ClientEntityRepository>();

            clientEntityRepositoryMock.Setup(
                mock => mock.Find(
                    clientEntityId
                )
            ).Returns(
                default(ClientEntity)
            );

            // When
            var handler = new DeleteClientEntityCommandHandler(
                mediatorMock.Object,
                clientEntityRepositoryMock.Object
            );
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

        [Fact]
        public async Task ShouldReturnFileNotFoundErrorCodeWhenEntityIsNotInRepository()
        {
            // Given
            var expected = ClientEntityErrorCodes.FILE_NOT_FOUND;
            var clientEntityId = "client-entity-id";
            var entityFileFullName = "entity-file-full-name";
            var clientEntity = new ClientEntity(
                clientEntityId,
                new ConcurrentDictionary<string, object>(
                    new Dictionary<string, object>
                    {
                        { ClientEntityConstants.METADATA_FILE_FULL_NAME, entityFileFullName },
                    }
                )
            );

            var mediatorMock = new Mock<IMediator>();
            var clientEntityRepositoryMock = new Mock<ClientEntityRepository>();

            clientEntityRepositoryMock.Setup(
                mock => mock.Find(
                    clientEntityId
                )
            ).Returns(
                clientEntity
            );

            // When
            var handler = new DeleteClientEntityCommandHandler(
                mediatorMock.Object,
                clientEntityRepositoryMock.Object
            );
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
    }
}