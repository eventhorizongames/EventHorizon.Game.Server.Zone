namespace EventHorizon.Zone.System.Player.Tests
{
    using EventHorizon.Game.Server.Zone;
    using EventHorizon.Test.Common;
    using EventHorizon.Test.Common.Utils;

    using Xunit;

    public class SystemPlayerExtensionsTests
    {
        [Fact]
        public void ShouldAddExpectedServices()
        {
            // Given
            var serviceCollectionMock = new ServiceCollectionMock();

            // When
            SystemPlayerExtensions.AddSystemPlayer(
                serviceCollectionMock
            );

            // Then
            Assert.Empty(
                serviceCollectionMock
            );
        }

        [Fact]
        public void ShouldReturnApplicationBuilderForChainingCommands()
        {
            // Given
            var applicationBuilderMocks = ApplicationBuilderFactory.CreateApplicationBuilder();
            var expected = applicationBuilderMocks.ApplicationBuilderMock.Object;

            // When
            var actual = SystemPlayerExtensions.UseSystemPlayer(
                applicationBuilderMocks.ApplicationBuilderMock.Object
            );

            // Then
            Assert.Equal(
                expected,
                actual
            );
        }
    }
}
