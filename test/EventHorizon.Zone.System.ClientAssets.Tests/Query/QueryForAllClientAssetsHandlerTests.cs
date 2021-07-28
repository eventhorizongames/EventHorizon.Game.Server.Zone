namespace EventHorizon.Zone.System.ClientAssets.Tests.Query
{
    using AutoFixture.Xunit2;
    using EventHorizon.Test.Common.Attributes;
    using EventHorizon.Zone.System.ClientAssets.Events.Query;
    using EventHorizon.Zone.System.ClientAssets.Model;
    using EventHorizon.Zone.System.ClientAssets.Query;
    using EventHorizon.Zone.System.ClientAssets.State.Api;
    using FluentAssertions;
    using global::System.Collections.Generic;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using Moq;
    using Xunit;

    public class QueryForAllClientAssetsHandlerTests
    {
        [Theory, AutoMoqData]
        public async Task ShouldReutrnAllFromAssetRepositoryWhenQueryIsHandled(
            // Given
            List<ClientAsset> expected,
            [Frozen] Mock<ClientAssetRepository> clientAssetRepositoryMock,
            QueryForAllClientAssetsHandler handler
        )
        {
            clientAssetRepositoryMock.Setup(
                mock => mock.All()
            ).Returns(
                expected
            );

            // When
            var actual = await handler.Handle(
                new QueryForAllClientAssets(),
                CancellationToken.None
            );

            // Then
            actual.Result.Should().BeEquivalentTo(expected);
        }
    }
}
