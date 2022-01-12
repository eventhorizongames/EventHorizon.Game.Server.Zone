namespace EventHorizon.Zone.System.ClientAssets.Tests.Query;

using AutoFixture.Xunit2;

using EventHorizon.Test.Common.Attributes;
using EventHorizon.Zone.System.ClientAssets.Api;
using EventHorizon.Zone.System.ClientAssets.Events.Query;
using EventHorizon.Zone.System.ClientAssets.Model;
using EventHorizon.Zone.System.ClientAssets.Query;

using FluentAssertions;

using global::System.Threading;
using global::System.Threading.Tasks;

using Moq;

using Xunit;

public class QueryForClientAssetByIdHandlerTests
{
    [Theory, AutoMoqData]
    public async Task ShouldReutrnClientAssetFromRepositoryWhenGetReturnsFilledOption(
        // Given
        ClientAsset expected,
        QueryForClientAssetById query,
        [Frozen]
            Mock<ClientAssetRepository> clientAssetRepositoryMock,
        QueryForClientAssetByIdHandler handler
    )
    {
        clientAssetRepositoryMock
            .Setup(mock => mock.Get(query.Id))
            .Returns(expected);

        // When
        var actual = await handler.Handle(
            query,
            CancellationToken.None
        );

        // Then
        actual.Success.Should().BeTrue();
        actual.Result.Should().Be(expected);
    }

    [Theory, AutoMoqData]
    public async Task ShouldReutrnErrorCommandResultFromRepositoryWhenRespositoryReturnsEmptyOption(
        // Given
        QueryForClientAssetById query,
        [Frozen]
            Mock<ClientAssetRepository> clientAssetRepositoryMock,
        QueryForClientAssetByIdHandler handler
    )
    {
        var expected = "CLIENT_ASSET_NOT_FOUND";
        clientAssetRepositoryMock
            .Setup(mock => mock.Get(query.Id))
            .Returns(new Option<ClientAsset>(null));

        // When
        var actual = await handler.Handle(
            query,
            CancellationToken.None
        );

        // Then
        actual.Success.Should().BeFalse();
        actual.ErrorCode.Should().Be(expected);
    }
}
