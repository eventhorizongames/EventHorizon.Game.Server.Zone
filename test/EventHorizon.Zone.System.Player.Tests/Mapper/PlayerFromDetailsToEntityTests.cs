using Xunit;
using System.Collections.Generic;
using System.Numerics;
using EventHorizon.Zone.System.Player.Model.Details;
using EventHorizon.Zone.System.Player.Model.Position;
using EventHorizon.Zone.System.Player.Mapper;

namespace EventHorizon.Zone.System.Player.Tests.Mapper
{
    public class PlayerFromDetailsToEntityTests
    {
        [Fact]
        public void TestShouldReturnAStandardPlayerEntityFromEntityDetails()
        {
            // Given
            var expectedId = -1;
            var expectedPlayerId = "123";
            var expectedConnectionId = "123-123-123";
            var expectedCurrentPosition = Vector3.Zero;
            var expectedCurrentZone = "current-zone";
            var expectedZoneTag = "zone-tag";
            var expectedData = new Dictionary<string, object>();
            expectedData.Add(
                "ConnectionId",
                expectedConnectionId
            );

            var expectedTag1 = "player";
            var input = new PlayerDetails
            {
                Id = expectedPlayerId,
                Position = new PlayerPositionState
                {
                    Position = expectedCurrentPosition,
                    CurrentZone = expectedCurrentZone,
                    ZoneTag = expectedZoneTag,
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
                expectedCurrentPosition,
                actual.Position.CurrentPosition
            );
            Assert.Equal(
                expectedCurrentPosition,
                actual.Position.MoveToPosition
            );
            Assert.Equal(
                expectedCurrentZone,
                actual.Position.CurrentZone
            );
            Assert.Equal(
                expectedZoneTag,
                actual.Position.ZoneTag
            );
            Assert.Collection(
                actual.TagList,
                tag => Assert.Equal(expectedTag1, tag)
            );
        }
        [Fact]
        public void TestShouldReturnPlayerEntityWithEmptyConnectionIdWhenDetailsDoesNotContainAConnectionId()
        {
            // Given
            var playerId = "player-id";
            var expected = "";
            var data = new Dictionary<string, object>();

            var input = new PlayerDetails
            {
                Id = playerId,
                Position = new PlayerPositionState
                {
                    
                },
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
}