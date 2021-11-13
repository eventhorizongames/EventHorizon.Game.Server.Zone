namespace EventHorizon.Zone.System.Interaction.Tests.Agent.Behavior
{
    using EventHorizon.Game.Server.Zone;
    using EventHorizon.Test.Common.Utils;

    using FluentAssertions;

    using Microsoft.AspNetCore.Builder;

    using Moq;

    using Xunit;
    using Xunit.Abstractions;

    public class SystemInteractionExtensionsTests
        : TestFixtureBase
    {
        public SystemInteractionExtensionsTests(
            ITestOutputHelper testOutputHelper
        ) : base(
            testOutputHelper
        )
        {
        }

        [Fact]
        public void TestAddServerSetup_ShouldAddExpectedServices()
        {
            // Given
            var serviceCollectionMock = new ServiceCollectionMock();

            // When
            SystemInteractionExtensions.AddSystemInteraction(
                serviceCollectionMock
            );

            // Then
            Assert.Empty(
                serviceCollectionMock
            );
        }

        [Fact]
        public void TestUseSetupServer_ShouldSendAndPublishExpectedEvent()
        {
            // Given
            var applicationBuilderMock = new Mock<IApplicationBuilder>();

            var expected = applicationBuilderMock.Object;

            // When
            var actual = SystemInteractionExtensions.UseSystemInteraction(
                applicationBuilderMock.Object
            );

            // Then
            actual.Should().Be(expected);
        }
    }
}
