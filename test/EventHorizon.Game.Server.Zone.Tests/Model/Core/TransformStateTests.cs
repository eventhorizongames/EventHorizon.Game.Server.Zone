namespace EventHorizon.Game.Server.Zone.Tests.Model.Core;

using System;
using System.Numerics;

using EventHorizon.Zone.Core.Model.Core;

using Xunit;

public class TransformStateTests
{
    [Fact]
    public void Test_ShouldHaveExpectedValues()
    {
        //Given
        var expectedPosition = Vector3.One;

        //When
        var actual = new TransformState
        {
            Position = expectedPosition,
        };

        //Then
        Assert.Equal(expectedPosition, actual.Position);
    }
}
