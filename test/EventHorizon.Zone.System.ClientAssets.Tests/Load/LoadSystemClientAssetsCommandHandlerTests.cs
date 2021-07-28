namespace EventHorizon.Zone.System.ClientAssets.Tests.Load
{
    using AutoFixture.Xunit2;
    using EventHorizon.Test.Common.Attributes;
    using EventHorizon.Zone.Core.Events.FileService;
    using EventHorizon.Zone.Core.Model.FileService;
    using EventHorizon.Zone.Core.Model.Json;
    using EventHorizon.Zone.System.ClientAssets.Add;
    using EventHorizon.Zone.System.ClientAssets.Load;
    using EventHorizon.Zone.System.ClientAssets.Model;
    using FluentAssertions;
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;
    using Moq;
    using Xunit;

    public class LoadSystemClientAssetsCommandHandlerTests
    {
        [Theory, AutoMoqData]
        public async Task ShouldPublishAddClientAssetEventWhenProcessingFileInfo(
            // Given
            [Frozen] Mock<IMediator> mediatorMock,
            [Frozen] Mock<IJsonFileLoader> fileLoaderMock,
            LoadSystemClientAssetsCommandHandler handler,

            StandardFileInfo fileInfo1,
            ClientAsset fileInfo1ClientAsset,
            StandardFileInfo fileInfo2,
            ClientAsset fileInfo2ClientAsset
        )
        {
            Func<StandardFileInfo, IDictionary<string, object>, Task> onProcessFile = null;
            IDictionary<string, object> arguments = null;

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

            fileLoaderMock.Setup(
                mock => mock.GetFile<ClientAsset>(
                    fileInfo1.FullName
                )
            ).ReturnsAsync(
                fileInfo1ClientAsset
            );

            fileLoaderMock.Setup(
                mock => mock.GetFile<ClientAsset>(
                    fileInfo2.FullName
                )
            ).ReturnsAsync(
                fileInfo2ClientAsset
            );

            // When
            await handler.Handle(
                new LoadSystemClientAssetsCommand(),
                CancellationToken.None
            );

            onProcessFile.Should().NotBeNull();
            arguments.Should().NotBeNull();

            // Then
            await onProcessFile(
                fileInfo1,
                arguments
            );
            await onProcessFile(
                fileInfo2,
                arguments
            );

            mediatorMock.Verify(
                mock => mock.Publish(
                    new AddClientAssetEvent(
                        fileInfo1ClientAsset
                    ),
                    CancellationToken.None
                )
            );
            mediatorMock.Verify(
                mock => mock.Publish(
                    new AddClientAssetEvent(
                        fileInfo2ClientAsset
                    ),
                    CancellationToken.None
                )
            );
        }
    }
}
