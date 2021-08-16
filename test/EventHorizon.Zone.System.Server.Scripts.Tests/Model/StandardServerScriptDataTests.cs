namespace EventHorizon.Zone.System.Server.Scripts.Tests.Model
{
    using EventHorizon.Zone.System.Server.Scripts.Run.Model;

    using FluentAssertions;

    using global::System.Collections.Generic;

    using Xunit;


    public class StandardServerScriptDataTests
    {
        [Fact]
        public void ShouldReturnExpectedDataWhenGetIsUsed()
        {
            // Given
            var key1 = "key-1";
            var value1 = new Dictionary<string, object>();
            var key2 = "key-2";
            var value2 = "value-2";

            var expectedValue1 = value1;
            var expectedValue2 = value2;

            // When
            var actual = new StandardServerScriptData(
                new Dictionary<string, object>
                {
                    [key1] = value1,
                    [key2] = value2,
                }
            );

            // Then
            actual.Get<IDictionary<string, object>>(
                key1
            ).Should().BeEquivalentTo(
                expectedValue1
            );
            actual.Get<string>(
                key2
            ).Should().Be(
                expectedValue2
            );
        }
    }
}
