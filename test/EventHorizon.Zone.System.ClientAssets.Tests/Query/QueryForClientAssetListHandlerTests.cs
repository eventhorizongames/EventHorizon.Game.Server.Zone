namespace EventHorizon.Zone.System.ClientAssets.Tests.Query;

using AutoFixture.Xunit2;

using EventHorizon.Test.Common.Attributes;
using EventHorizon.Zone.System.ClientAssets.Api;
using EventHorizon.Zone.System.ClientAssets.Model;
using EventHorizon.Zone.System.ClientAssets.Query;

using FluentAssertions;

using global::System.Collections.Generic;
using global::System.Threading;
using global::System.Threading.Tasks;

using Moq;

using Xunit;

public class QueryForClientAssetListHandlerTests
{
    [Theory, AutoMoqData]
    public async Task ShouldReutrnAllFromAssetRepositoryWhenQueryIsHandled(
        // Given
        List<ClientAsset> expected,
        [Frozen]
            Mock<ClientAssetRepository> clientAssetRepositoryMock,
        QueryForClientAssetListHandler handler
    )
    {
        clientAssetRepositoryMock
            .Setup(mock => mock.All())
            .Returns(expected);

        // When
        var actual = await handler.Handle(
            new QueryForClientAssetList(),
            CancellationToken.None
        );

        // Then
        actual.Should().BeEquivalentTo(expected);
    }
}
