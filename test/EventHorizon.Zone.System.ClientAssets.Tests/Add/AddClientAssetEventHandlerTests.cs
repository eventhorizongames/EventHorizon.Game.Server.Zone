namespace EventHorizon.Zone.System.ClientAssets.Tests.Add
{
    using AutoFixture.Xunit2;

    using EventHorizon.Test.Common.Attributes;
    using EventHorizon.Zone.System.ClientAssets.Add;
    using EventHorizon.Zone.System.ClientAssets.State.Api;

    using global::System.Threading;
    using global::System.Threading.Tasks;

    using Moq;

    using Xunit;

    public class AddClientAssetEventHandlerTests
    {
        [Theory, AutoMoqData]
        public async Task ShoudlAddClientRepositoryWhenEventIsHandled(
            // Given
            AddClientAssetEvent notification,
            [Frozen] Mock<ClientAssetRepository> clientAssetRepositoryMock,
            AddClientAssetEventHandler handler
        )
        {
            // When
            await handler.Handle(
                notification,
                CancellationToken.None
            );

            // Then
            clientAssetRepositoryMock.Verify(
                mock => mock.Add(
                    notification.ClientAsset
                )
            );
        }
    }
}
