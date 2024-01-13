namespace EventHorizon.Game.Server.Zone.Tests.Model.Entity;

using EventHorizon.Zone.Core.Model.Entity;

using Xunit;

public class EntityActionTests
{
    [Fact]
    public void TestEquals_WhenInputIsNullShouldReturnFalse()
    {
        //Given
        string input = null;

        //When
        var entityAction = EntityAction.POSITION;

        var actual = entityAction.Equals(input);

        //Then
        Assert.False(actual);
    }

    [Fact]
    public void TestEquals_WhenInputIsNotEntityActionTypeShouldReturnFalse()
    {
        // Given
        string input = "not-entity-action";

        // When
        var entityAction = EntityAction.POSITION;

        var actual = entityAction.Equals(input);

        // Then
        Assert.False(actual);
    }

    [Fact]
    public void TestEquals_WhenInputIsEntityActionNotSameIdShouldReturnFalse()
    {
        // Given
        var input = EntityAction.POSITION;

        // When
        var entityAction = EntityAction.ADD;

        var actual = entityAction.Equals(input);

        // Then
        Assert.False(actual);
    }

    [Fact]
    public void TestEquals_WhenInputIsEntityActionSameIdShouldReturnTrue()
    {
        // Given
        var input = EntityAction.POSITION;

        // When
        var entityAction = EntityAction.POSITION;

        var actual = entityAction.Equals(input);

        // Then
        Assert.True(actual);
    }

    [Fact]
    public void TestGetHashCode_ShouldReturnTheHashCodeOfTheTypeString()
    {
        // Given
        var inputId = "Entity.Add";
        var expected = inputId.GetHashCode();

        // When
        var entityAction = EntityAction.ADD;

        var actual = entityAction.GetHashCode();

        // Then
        Assert.Equal(expected, actual);
    }
}
