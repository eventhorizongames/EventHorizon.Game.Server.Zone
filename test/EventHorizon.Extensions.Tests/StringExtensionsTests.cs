namespace EventHorizon.Extensions.Tests;

using FluentAssertions;

using Xunit;

public class StringExtensionsTests
{
    [Theory]
    [InlineData("", 371857150)]
    [InlineData("hash-code", 739418864)]
    [InlineData("AAABBBCCCddddeeefff", -697831353)]
    [InlineData("aaaabbb123456789", -312674781)]
    [InlineData("1234567890", -1505891123)]
    [InlineData("12345asdfqwer", 459731324)]
    [InlineData("Hello World!!@#$%^&*()", 269785881)]
    public void ShouldReturnExpectedHashCodeWhenUsingDeterministicHashCodeExtension(
        // Given
        string str,
        int expected
    )
    {
        // When
        var actual = str.GetDeterministicHashCode();

        // Then
        actual.Should().Be(expected);
    }
}
