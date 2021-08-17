namespace EventHorizon.Extensions.Tests
{
    using System;

    using FluentAssertions;

    using Xunit;

    public class ObjectExtensionsTests
    {
        [Fact]
        [Trait("Category", "DebugOnly")]
        public void ShouldThrowExceptionWhenNull()
        {
            // Given
            string nullObj = null;

            // When
            Action action = () => nullObj.ValidateForNull();

            var actual = action.Should().Throw<NullReferenceException>();

            // Then
            actual.WithMessage("Found a Null");
        }
    }
}
