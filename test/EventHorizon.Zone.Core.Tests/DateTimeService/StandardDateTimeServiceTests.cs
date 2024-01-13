namespace EventHorizon.Zone.Core.Tests.DateTimeService;

using System;

using EventHorizon.Zone.Core.DateTimeService;

using Xunit;

public class StandardDateTimeServiceTests
{
    [Fact]
    public void TestShouldReturnUtcDateTimeWhenNowIsCalled()
    {
        // Given
        var expected = DateTimeKind.Utc;

        // When
        var dateTimeService = new StandardDateTimeService();
        var actual = dateTimeService.Now;

        // Then
        Assert.Equal(
            expected,
            actual.Kind
        );
    }
}
