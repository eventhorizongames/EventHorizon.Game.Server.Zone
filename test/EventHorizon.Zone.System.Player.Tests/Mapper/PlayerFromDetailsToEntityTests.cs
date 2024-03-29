namespace EventHorizon.Zone.System.Player.Tests.Mapper;

using EventHorizon.Zone.Core.Model.Core;
using EventHorizon.Zone.System.Player.Mapper;
using EventHorizon.Zone.System.Player.Model.Details;

using global::System.Collections.Concurrent;
using global::System.Numerics;

using Xunit;

public class PlayerFromDetailsToEntityTests
{
    [Fact]
    public void ShouldReturnAStandardPlayerEntityFromEntityDetails()
    {
        // Given
        var expectedId = -1;
        var expectedPlayerId = "123";
        var expectedConnectionId = "123-123-123";
        var expectedPosition = Vector3.Zero;
        var expectedData = new ConcurrentDictionary<string, object>();
        expectedData.TryAdd(
            "ConnectionId",
            expectedConnectionId
        );

        var expectedTag1 = "player";
        var input = new PlayerDetails
        {
            Id = expectedPlayerId,
            Transform = new TransformState
            {
                Position = expectedPosition,
            },
            Data = expectedData
        };

        // When
        var actual = PlayerFromDetailsToEntity.MapToNew(
            input
        );

        // Then
        Assert.Equal(
            expectedId,
            actual.Id
        );
        Assert.Equal(
            expectedPlayerId,
            actual.PlayerId
        );
        Assert.Equal(
            expectedConnectionId,
            actual.ConnectionId
        );
        Assert.Equal(
            expectedPosition,
            actual.Transform.Position
        );
        Assert.Collection(
            actual.TagList,
            tag => Assert.Equal(expectedTag1, tag)
        );
    }
    [Fact]
    public void ShouldReturnPlayerEntityWithEmptyConnectionIdWhenDetailsDoesNotContainAConnectionId()
    {
        // Given
        var playerId = "player-id";
        var expected = "";
        var data = new ConcurrentDictionary<string, object>();

        var input = new PlayerDetails
        {
            Id = playerId,
            Data = data
        };

        // When
        var actual = PlayerFromDetailsToEntity.MapToNew(
            input
        );

        // Then
        Assert.Equal(
            expected,
            actual.ConnectionId
        );
    }
}
