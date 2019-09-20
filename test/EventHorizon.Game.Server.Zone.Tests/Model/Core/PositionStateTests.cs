using System;
using System.Numerics;
using EventHorizon.Zone.Core.Model.Core;
using Xunit;

namespace EventHorizon.Game.Server.Zone.Tests.Model.Core
{
    public class PositionStateTests
    {
        [Fact]
        public void Test_ShouldHaveExpectedValues()
        {
            //Given
            var expectedCurrentPosition = Vector3.One;
            var expectedNextMoveRequest = DateTime.Now;
            var expectedMoveToPosition = Vector3.Zero;
            var expectedCurrentZone = "my zone";
            var expectedZoneTag = "zone-tag";

            //When
            var actual = new PositionState
            {
                CurrentPosition = expectedCurrentPosition,
                NextMoveRequest = expectedNextMoveRequest,
                MoveToPosition = expectedMoveToPosition,
                CurrentZone = expectedCurrentZone,
                ZoneTag = expectedZoneTag
            };

            //Then
            Assert.Equal(expectedCurrentPosition, actual.CurrentPosition);
            Assert.Equal(expectedNextMoveRequest, actual.NextMoveRequest);
            Assert.Equal(expectedMoveToPosition, actual.MoveToPosition);
            Assert.Equal(expectedCurrentZone, actual.CurrentZone);
            Assert.Equal(expectedZoneTag, actual.ZoneTag);
        }
    }
}