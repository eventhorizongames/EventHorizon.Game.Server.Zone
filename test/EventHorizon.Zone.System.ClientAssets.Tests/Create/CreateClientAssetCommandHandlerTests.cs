namespace EventHorizon.Zone.System.ClientAssets.Tests.Create
{
    using AutoFixture.Xunit2;

    using EventHorizon.Test.Common.Attributes;
    using EventHorizon.Zone.System.ClientAssets.Create;
    using EventHorizon.Zone.System.ClientAssets.Events.Create;
    using EventHorizon.Zone.System.ClientAssets.Save;
    using EventHorizon.Zone.System.ClientAssets.State.Api;

    using FluentAssertions;

    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    using Moq;

    using Xunit;

    public class CreateClientAssetCommandHandlerTests
    {
        [Theory, AutoMoqData]
        public async Task ShoudlSetClientAssetInRepositoryWhenCommandIsHandled(
            // Given
            CreateClientAssetCommand request,
            [Frozen] Mock<IMediator> mediatorMock,
            [Frozen] Mock<ClientAssetRepository> clientAssetRepositoryMock,
            CreateClientAssetCommandHandler handler
        )
        {
            // When
            var actual = await handler.Handle(
                request,
                CancellationToken.None
            );

            // Then
            actual.Success
                .Should().BeTrue();

            mediatorMock.Verify(
                mock => mock.Publish(
                    new RunSaveClientAssetsEvent(),
                    CancellationToken.None
                )
            );
            clientAssetRepositoryMock.Verify(
                mock => mock.Set(
                    request.ClientAsset
                )
            );
        }

        [Theory, AutoMoqData]
        public async Task ShoudlNotSetFileFullNameWhenFoundOnClientAsset(
            // Given
            CreateClientAssetCommand request,
            string rootPath,
            CreateClientAssetCommandHandler handler
        )
        {
            var clientAsset = request.ClientAsset;
            var expected = clientAsset.GetFileFullName(
                rootPath
            );
            clientAsset.SetFileFullName(
                expected
            );

            // When
            await handler.Handle(
                request,
                CancellationToken.None
            );
            clientAsset.TryGetFileFullName(
                out var actual
            ).Should().BeTrue();

            // Then
            actual.Should().Be(expected);
        }
    }
}
