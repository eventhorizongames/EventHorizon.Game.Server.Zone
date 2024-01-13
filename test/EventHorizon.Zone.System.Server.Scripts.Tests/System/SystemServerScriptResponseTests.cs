namespace EventHorizon.Zone.System.Server.Scripts.Tests.System;

using EventHorizon.Zone.System.Server.Scripts.System;

using FluentAssertions;

using Xunit;


public class SystemServerScriptResponseTests
{
    [Fact]
    public void ShouldReturnExpectedDataWhenCreated()
    {
        // Given
        var success = true;
        var message = "message";

        var expectedSuccess = success;
        var expectedMessage = message;


        // When
        var actual = new SystemServerScriptResponse(
            success,
            message
        );

        // Then
        actual.Success
            .Should().Be(expectedSuccess);
        actual.Message
            .Should().Be(expectedMessage);
    }
}
