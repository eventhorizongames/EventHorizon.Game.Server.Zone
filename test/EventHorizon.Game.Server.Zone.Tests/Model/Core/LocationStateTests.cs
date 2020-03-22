namespace EventHorizon.Game.Server.Zone.Tests.Model.Core
{
    using System;
    using System.Numerics;
    using EventHorizon.Zone.Core.Model.Core;
    using Xunit;

    public class LocationStateTests
    {
        [Fact]
        public void Test_ShouldHaveExpectedValues()
        {
            //Given
            var expectedNextMoveRequest = DateTime.Now;
            var expectedMoveToPosition = Vector3.Zero;
            var expectedCurrentZone = "my zone";
            var expectedZoneTag = "zone-tag";

            //When
            var actual = new LocationState
            {
                NextMoveRequest = expectedNextMoveRequest,
                MoveToPosition = expectedMoveToPosition,
                CurrentZone = expectedCurrentZone,
                ZoneTag = expectedZoneTag
            };

            //Then
            Assert.Equal(expectedNextMoveRequest, actual.NextMoveRequest);
            Assert.Equal(expectedMoveToPosition, actual.MoveToPosition);
            Assert.Equal(expectedCurrentZone, actual.CurrentZone);
            Assert.Equal(expectedZoneTag, actual.ZoneTag);
        }
    }
}