namespace EventHorizon.Extensions.Tests
{
    using System;

    using FluentAssertions;

    using Xunit;

    public class StringExtensionsTests
    {
        [Fact]
        public void ShouldUppercaseFirstLetterWhenStringLetterIsLowercased()
        {
            // Given
            var input = "stringhellowworld";
            var expected = "Stringhellowworld";

            // When
            var actual = input.UppercaseFirstChar();

            // Then
            actual.Should().Be(expected);
        }

        [Fact]
        public void ShouldLowercaseFirstLetterWhenStringLetterIsUppercased()
        {
            // Given
            var input = "STRINGHELLOWORLD";
            var expected = "sTRINGHELLOWORLD";

            // When
            var actual = input.LowercaseFirstChar();

            // Then
            actual.Should().Be(expected);
        }

        [Fact]
        public void ShouldReturnEmptyStringWhenTextIsNullOnLowercaseFirstCharIsCalled()
        {
            // Given
            string input = null;
            var expected = string.Empty;

            // When
            var actual = input.LowercaseFirstChar();

            // Then
            actual.Should().Be(expected);
        }

        [Fact]
        public void ShouldReturnEmptyStringWhenTextIsNullOnUppercaseFirstCharIsCalled()
        {
            // Given
            string input = null;
            var expected = string.Empty;

            // When
            var actual = input.UppercaseFirstChar();

            // Then
            actual.Should().Be(expected);
        }

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
}
