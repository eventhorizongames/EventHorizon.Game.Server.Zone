namespace EventHorizon.Zone.System.Server.Scripts.Tests.State
{

    using AutoFixture.Xunit2;

    using EventHorizon.Zone.System.Server.Scripts.Plugin.Shared.Model;
    using EventHorizon.Zone.System.Server.Scripts.State;

    using FluentAssertions;

    using global::System.Collections.Generic;

    using Xunit;


    public class StandardServerScriptsStateTests
    {
        [Fact]
        public void ShouldHaveEmptyCurrentHashWhenFirstCreated()
        {
            // Given
            var expected = string.Empty;

            // When
            var state = new StandardServerScriptsState();
            var actual = state.CurrentHash;

            // Then
            actual
                .Should().Be(expected);
        }

        [Fact]
        public void ShouldSetCurrentHasWhenUpdateHashIsCalled()
        {
            // Given
            var hash = "hash";
            var expected = hash;

            // When
            var state = new StandardServerScriptsState();
            state.UpdateHash(
                hash
            );

            var actual = state.CurrentHash;

            // Then
            actual
                .Should().Be(expected);
        }

        [Theory, AutoData]
        public void ShouldClearCurrentHashWhenErrorCodeIsSet(
            // Given
            string errorCode,
            StandardServerScriptsState state
        )
        {
            var expected = string.Empty;

            // When
            state.SetErrorCode(
                errorCode
            );
            var actual = state.CurrentHash;

            // Then
            state.ErrorCode.Should().Be(
                errorCode
            );
            actual.Should().Be(
                expected
            );
        }

        [Theory, AutoData]
        public void ShouldClearCurrentHashWhenErrorStateIsSet(
            // Given
            List<GeneratedServerScriptErrorDetailsModel> errorDetailsList,
            StandardServerScriptsState state
        )
        {
            var expected = string.Empty;

            // When
            state.SetErrorState(
                errorDetailsList
            );
            var actual = state.CurrentHash;

            // Then
            state.ErrorDetailsList.Should().BeEquivalentTo(
                errorDetailsList
            );
            actual.Should().Be(
                expected
            );
        }
    }
}
