namespace EventHorizon.Server.Core.Connection.Tests.Model;

using EventHorizon.Server.Core.Connection.Model;

using Xunit;

public class RegisteredZoneDetailsTests
{
    [Fact]
    public void ShouldValidateIdWhenSet()
    {
        // Given
        var expected = "id-1000";

        // When
        var details = new RegisteredZoneDetails
        {
            Id = expected
        };

        // Then
        Assert.Equal(
            expected,
            details.Id
        );
    }
}
