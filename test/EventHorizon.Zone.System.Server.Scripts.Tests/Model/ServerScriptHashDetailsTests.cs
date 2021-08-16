namespace EventHorizon.Zone.System.Server.Scripts.Tests.Model
{
    using EventHorizon.Zone.System.Server.Scripts.Model;

    using FluentAssertions;

    using Xunit;


    public class ServerScriptHashDetailsTests
    {
        [Fact]
        public void ShouldReturnExpectedDataWhenCreated()
        {
            // Given
            var isDirty = true;
            var hash = "hash";

            var expectedIsDirty = isDirty;
            var expectedHash = hash;

            // When
            var actual = new ServerScriptHashDetails(
                isDirty,
                hash
            );

            // Then
            actual.IsDirty
                .Should().Be(expectedIsDirty);
            actual.Hash
                .Should().Be(expectedHash);
        }
    }
}
