namespace EventHorizon.Zone.Core.Tests.Json;

using EventHorizon.Zone.Core.Json;

using FluentAssertions;

using Xunit;

public class NewtonsoftSerializeToJsonServiceTests
{
    [Fact]
    public void ShouldReturnNotEmptyStringWhenObjectSerialized()
    {
        // Given
        var objectToSerialize = new
        {
            Id = 1,
        };


        // When
        var service = new NewtonsoftSerializeToJsonService();
        var actual = service.Serialize(
            objectToSerialize
        );

        // Then
        actual.Should().NotBeNullOrEmpty();
    }
}
