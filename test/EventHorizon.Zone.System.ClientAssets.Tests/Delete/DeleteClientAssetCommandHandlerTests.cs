namespace EventHorizon.Zone.System.ClientAssets.Tests.Delete
{
    using AutoFixture.Xunit2;

    using EventHorizon.Test.Common.Attributes;
    using EventHorizon.Zone.Core.Events.FileService;
    using EventHorizon.Zone.System.Backup.Events;
    using EventHorizon.Zone.System.ClientAssets.Delete;
    using EventHorizon.Zone.System.ClientAssets.Events.Delete;
    using EventHorizon.Zone.System.ClientAssets.Model;
    using EventHorizon.Zone.System.ClientAssets.State.Api;

    using FluentAssertions;

    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    using Moq;

    using Xunit;

    public class DeleteClientAssetCommandHandlerTests
    {
        [Theory, AutoMoqData]
        public async Task ShouldCallDeleteOnClientAssetRepositoryWhenRequestIsValid(
            // Given
            ClientAsset clientAsset,
            DeleteClientAssetCommand request,
            [Frozen] Mock<ClientAssetRepository> clientAssetRepositoryMock,
            DeleteClientAssetCommandHandler handler
        )
        {
            clientAssetRepositoryMock.Setup(
                mock => mock.Get(
                    request.Id
                )
            ).Returns(
                clientAsset
            );

            // When
            var actual = await handler.Handle(
                request,
                CancellationToken.None
            );

            // Then
            actual.Success
                .Should().BeTrue();

            clientAssetRepositoryMock.Verify(
                mock => mock.Delete(
                    request.Id
                )
            );
        }

        [Theory, AutoMoqData]
        public async Task ShouldNotCallDeleteOnClientAssetRepositoryWhenRequetedIdIsNotFound(
            // Given
            DeleteClientAssetCommand request,
            [Frozen] Mock<ClientAssetRepository> clientAssetRepositoryMock,
            DeleteClientAssetCommandHandler handler
        )
        {
            clientAssetRepositoryMock.Setup(
                mock => mock.Get(
                    It.IsAny<string>()
                )
            ).Returns(
                new Option<ClientAsset>(null)
            );

            // When
            var actual = await handler.Handle(
                request,
                CancellationToken.None
            );

            // Then
            actual.Success
                .Should().BeTrue();

            clientAssetRepositoryMock.Verify(
                mock => mock.Delete(
                    It.IsAny<string>()
                ),
                Times.Never()
            );
        }

        [Theory, AutoMoqData]
        public async Task ShouldCreateBackupAndDeleteFileWhenTryGetFileFullNameIsFoundOnClientAssetFromRepository(
            // Given
            ClientAsset clientAsset,
            string assetFileFullName,
            DeleteClientAssetCommand request,
            [Frozen] Mock<IMediator> mediatorMock,
            [Frozen] Mock<ClientAssetRepository> clientAssetRepositoryMock,
            DeleteClientAssetCommandHandler handler
        )
        {
            var createBackupOfFileCommand = new CreateBackupOfFileCommand(
                assetFileFullName
            );
            var deleteFile = new DeleteFile(
                assetFileFullName
            );

            clientAsset.SetFileFullName(
                assetFileFullName
            );

            clientAssetRepositoryMock.Setup(
                mock => mock.Get(
                    request.Id
                )
            ).Returns(
                clientAsset
            );

            // When
            var actual = await handler.Handle(
                request,
                CancellationToken.None
            );

            // Then
            actual.Success
                .Should().BeTrue();

            mediatorMock.Verify(
                mock => mock.Send(
                    createBackupOfFileCommand,
                    CancellationToken.None
                )
            );

            mediatorMock.Verify(
                mock => mock.Send(
                    deleteFile,
                    CancellationToken.None
                )
            );
        }
    }
}
