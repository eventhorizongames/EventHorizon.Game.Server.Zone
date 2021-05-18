namespace EventHorizon.Zone.System.Server.Scripts.Tests.StateManagement
{
    using EventHorizon.Zone.System.Server.Scripts.StateManagement;
    using FluentAssertions;
    using Xunit;


    public class KeyValueServerScriptRuntimeStateTests
    {
        [Fact]
        public void ShouldAddNewValueWhenStateDoesNotAlreadyContainValue()
        {
            // Given
            var key = "key";

            var expected = "value";

            // When
            var keyValueServerScriptRuntimeState = new KeyValueServerScriptRuntimeState();
            keyValueServerScriptRuntimeState.TryGetValue<string>(
                key, out _
            ).Should().BeFalse();
            keyValueServerScriptRuntimeState.AddOrUpdate(
                key,
                expected
            );

            // Then
            keyValueServerScriptRuntimeState.TryGetValue<string>(
                key,
                out var actual
            ).Should().BeTrue();
            actual.Should().Be(expected);
        }

        [Fact]
        public void ShouldAddUpdateValueWhenStateDoesAlreadyContainValue()
        {
            // Given
            var key = "key";
            var value = "initial-value";

            var expected = "value";

            // When
            var keyValueServerScriptRuntimeState = new KeyValueServerScriptRuntimeState();
            keyValueServerScriptRuntimeState.AddOrUpdate(
                key,
                value
            );
            keyValueServerScriptRuntimeState.TryGetValue<string>(
                key,
                out var initialValue
            ).Should().BeTrue();
            initialValue
                .Should().Be(value);

            // Add expected value
            keyValueServerScriptRuntimeState.AddOrUpdate(
                key,
                expected
            );

            // Then
            keyValueServerScriptRuntimeState.TryGetValue<string>(
                key,
                out var actual
            ).Should().BeTrue();
            actual.Should().Be(expected);
        }

        [Fact]
        public void ShouldReturnDefaultWhenCastIsOfADifferentType()
        {
            // Given
            var key = "key";
            var initialValue = "123";

            var expected = 0;

            // When
            var keyValueServerScriptRuntimeState = new KeyValueServerScriptRuntimeState();
            keyValueServerScriptRuntimeState.AddOrUpdate(
                key,
                initialValue
            );

            // Then
            keyValueServerScriptRuntimeState.TryGetValue<int>(
                key,
                out var actual
            ).Should().BeFalse();
            actual.Should().Be(expected);
        }

        [Fact]
        public void ShouldReturnNullWhenInvalidCastExceptionCouldNotFixValue()
        {
            // Given
            var key = "key";
            var initialValue = 123;

            var keyValueServerScriptRuntimeState = new KeyValueServerScriptRuntimeState();
            keyValueServerScriptRuntimeState.AddOrUpdate(
                key,
                initialValue
            );

            // Then
            keyValueServerScriptRuntimeState.TryGetValue<string>(
                key,
                out var actual
            ).Should().BeFalse();
            actual.Should().BeNull();
        }



        [Fact]
        public void ShouldReturnNewCastTypeWhenCastingToTwoSamePropertyTypedTypes()
        {
            // Given
            var key = "key";
            var count = 123;

            var expected = count;

            var initialValue = new TestingInitialStateType
            {
                Count = count,
            };

            var keyValueServerScriptRuntimeState = new KeyValueServerScriptRuntimeState();
            keyValueServerScriptRuntimeState.AddOrUpdate(
                key,
                initialValue
            );

            // Then
            keyValueServerScriptRuntimeState.TryGetValue<TestingNewStateType>(
                key,
                out var actual
            ).Should().BeTrue();
            actual.Count
                .Should().Be(expected);
        }

        private class TestingInitialStateType
        {
            public int Count { get; set; }
        }

        private class TestingNewStateType
        {
            public int Count { get; set; }
        }
    }
}
