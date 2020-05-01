namespace EventHorizon.Extensions.Tests
{
    using System;
    using System.Threading.Tasks;
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
    }
}
