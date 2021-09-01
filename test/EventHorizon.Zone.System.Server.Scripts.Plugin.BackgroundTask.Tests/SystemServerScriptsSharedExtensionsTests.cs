namespace EventHorizon.Zone.System.Server.Scripts.Plugin.BackgroundTask.Tests
{
    using EventHorizon.Game.Server.Zone;
    using EventHorizon.Test.Common;
    using EventHorizon.Test.Common.Utils;
    using EventHorizon.Zone.System.Server.Scripts.Plugin.BackgroundTask.Api;
    using EventHorizon.Zone.System.Server.Scripts.Plugin.BackgroundTask.Builders;
    using EventHorizon.Zone.System.Server.Scripts.Plugin.BackgroundTask.State;

    using FluentAssertions;

    using Xunit;

    public class SystemServerScriptsPluginSharedExtensionsTests
    {
        [Fact]
        public void ShouldRegisterExpectedServices()
        {
            // Given
            var serviceCollectionMock = new ServiceCollectionMock();

            // When
            SystemServerScriptsPluginBackgroundTaskExtensions.AddSystemServerScriptsPluginBackgroundTask(
                serviceCollectionMock
            );

            // Then
            serviceCollectionMock.Should().Contain(
                service => typeof(BackgroundTaskWrapperRepository) == service.ServiceType
                    && typeof(InMemoryBackgroundTaskWrapperRepository) == service.ImplementationType
            );
            serviceCollectionMock.Should().Contain(
                service => typeof(BackgroundTaskWrapperBuilder) == service.ServiceType
                    && typeof(ThreadedBackgroundTaskWrapperBuilder) == service.ImplementationType
            );
        }

        [Fact]
        public void ShouldReturnPassedInApplicationBuilderWhenFinishedRunning()
        {
            // Given
            var applicationBuilderMocks = ApplicationBuilderFactory.CreateApplicationBuilder();
            var expected = applicationBuilderMocks.ApplicationBuilderMock.Object;

            // When
            var actual = SystemServerScriptsPluginBackgroundTaskExtensions.UseSystemServerScriptsPluginBackgroundTask(
                applicationBuilderMocks.ApplicationBuilderMock.Object
            );

            // Then
            actual
                .Should().Be(expected);
        }
    }
}
