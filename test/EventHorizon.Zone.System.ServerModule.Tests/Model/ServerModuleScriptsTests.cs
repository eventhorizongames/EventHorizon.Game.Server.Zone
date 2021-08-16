namespace EventHorizon.Zone.System.ServerModule.Tests.Model
{
    using EventHorizon.Zone.System.ServerModule.Model;

    using FluentAssertions;

    using Xunit;

    public class ServerModuleScriptsTests
    {
        [Fact]
        public void ShouldHaveExpectedValuesWhenCreated()
        {
            // Given
            var expectedName = "Name";
            var expectedInitializeScript = "InitializeScript";
            var expectedDisposeScript = "DisposeScript";
            var expectedUpdateScript = "UpdateScript";

            // When
            var actual = new ServerModuleScripts
            {
                Name = expectedName,
                InitializeScript = expectedInitializeScript,
                DisposeScript = expectedDisposeScript,
                UpdateScript = expectedUpdateScript,
            };

            // Then
            actual.Name
                .Should().Be(expectedName);
            actual.InitializeScript
                .Should().Be(expectedInitializeScript);
            actual.DisposeScript
                .Should().Be(expectedDisposeScript);
            actual.UpdateScript
                .Should().Be(expectedUpdateScript);
        }
    }
}
