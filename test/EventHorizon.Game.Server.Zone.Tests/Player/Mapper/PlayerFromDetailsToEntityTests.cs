using Xunit;
using Moq;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Core.Player.Model;
using System.Collections.Generic;
using System.Numerics;
using EventHorizon.Game.Server.Zone.Player.Mapper;
using System.Dynamic;

namespace EventHorizon.Game.Server.Zone.Tests.Player.Mapper
{
    public class PlayerFromDetailsToEntityTests
    {
        [Fact]
        public void TestMapToNew_ShouldReturnAStandardPlayerEntityFromEntityDetails()
        {
            // Given
            var expectedId = -1;
            var expectedPlayerId = "123";
            var expectedConnectionId = "123-123-123";
            var expectedCurrentPosition = Vector3.Zero;
            var expectedCurrentZone = "current-zone";
            var expectedZoneTag = "zone-tag";
            dynamic expectedData = new ExpandoObject();
            expectedData.ConnectionId = expectedConnectionId;

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
            var actual = PlayerFromDetailsToEntity.MapToNew(input);

            // Then
            Assert.Equal(expectedId, actual.Id);
            Assert.Equal(expectedPlayerId, actual.PlayerId);
            Assert.Equal(expectedConnectionId, actual.ConnectionId);
            Assert.Equal(expectedCurrentPosition, actual.Position.CurrentPosition);
            Assert.Equal(expectedCurrentPosition, actual.Position.MoveToPosition);
            Assert.Equal(expectedCurrentZone, actual.Position.CurrentZone);
            Assert.Equal(expectedZoneTag, actual.Position.ZoneTag);
            Assert.Collection(actual.TagList,
                 a => Assert.Equal(expectedTag1, a)
            );

        }
    }
}