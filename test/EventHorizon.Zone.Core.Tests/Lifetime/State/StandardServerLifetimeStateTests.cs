using System.Threading.Tasks;
using EventHorizon.Zone.Core.Lifetime.State;
using FluentAssertions;
using Xunit;

namespace EventHorizon.Zone.Core.Tests.Lifetime.State
{
    public class StandardServerLifetimeStateTests
    {
        [Fact]
        public void ShouldNotBeStartedWhenFirstCreated()
        {
            // Given

            // When
            var state = new StandardServerLifetimeState();
            var actual = state.IsServerStarted();
            
            // Then
            actual.Should()
                .BeFalse();
        }

        [Fact]
        public void ShouldIndicatedTrueWhenSetIsCalledWithTrue()
        {
            // Given
            var expected = true;

            // When
            var state = new StandardServerLifetimeState();
            state.SetServerStarted(
                expected
            );
            var actual = state.IsServerStarted();
            
            // Then
            actual.Should()
                .Be(
                    expected
                );
        }
    }
}