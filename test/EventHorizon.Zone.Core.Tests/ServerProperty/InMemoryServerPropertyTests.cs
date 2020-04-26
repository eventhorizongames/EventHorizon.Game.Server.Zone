using EventHorizon.Zone.Core.ServerProperty;
using Xunit;

namespace EventHorizon.Zone.Core.Tests.ServerProperty
{
    public class InMemoryServerPropertyTests
    {
        [Fact]
        public void ShouldGetSamePropertyValueWhenPropertyIsAddedByKey()
        {
            // Given
            var key = "key";
            var value = "this-is-a-string";
            var expected = value;

            // When
            var serverProperty = new InMemoryServerProperty();
            serverProperty.Set(
                key,
                value
            );

            var actual = serverProperty.Get<string>(
                key
            );

            // Then
            Assert.Equal(
                expected,
                actual
            );
        }

        [Fact]
        public void TestShouldGetDefaultValueWhenNotFound()
        {
            // Given
            var key = "key";
            var expected = default(string);

            // When
            var serverProperty = new InMemoryServerProperty();

            var actual = serverProperty.Get<string>(
                key
            );

            // Then
            Assert.Equal(
                expected,
                actual
            );
        }

        [Fact]
        public void ShouldGetLastPropertyAddedWhenMultiplePropertiesAreAddedAtSameKey()
        {
            // Given
            var key = "key";
            var firstValue = "this-is-the-first-string";
            var value = "this-is-the-expected-string";
            var expected = value;

            // When
            var serverProperty = new InMemoryServerProperty();
            serverProperty.Set(
                key,
                firstValue
            );
            serverProperty.Set(
                key,
                value
            );

            var actual = serverProperty.Get<string>(
                key
            );

            // Then
            Assert.Equal(
                expected,
                actual
            );
        }
    }
}