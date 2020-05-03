namespace EventHorizon.Zone.Core.Map.Plugin.Editor.Tests
{
    using EventHorizon.Game.Server.Zone;
    using EventHorizon.Test.Common.Utils;
    using FluentAssertions;
    using Xunit;

    public class CoreMapPluginEditorExtensionsTests
    {
        [Fact]
        public void ShouldReturnPassedInCall()
        {
            // Given
            var serviceCollectionMock = new ServiceCollectionMock();
            var expected = serviceCollectionMock;

            // When
            var actual = CoreMapPluginEditorExtensions.AddCoreMapPluginEditor(
                serviceCollectionMock
            );

            // Then
            actual.Should().BeEquivalentTo(expected);
        }
    }
}
