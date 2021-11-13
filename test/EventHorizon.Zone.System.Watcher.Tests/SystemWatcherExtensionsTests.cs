namespace EventHorizon.Zone.System.Admin.Plugin.Command.Tests
{
    using EventHorizon.Game.Server.Zone;
    using EventHorizon.Test.Common;
    using EventHorizon.Test.Common.Utils;
    using EventHorizon.TimerService;
    using EventHorizon.Zone.System.Watcher.State;
    using EventHorizon.Zone.System.Watcher.Timer;

    using Xunit;

    public class SystemAdminPluginCommandExtensionsTests
    {
        [Fact]
        public void TestShouldConfigurationServiceCollection()
        {
            // Given
            var serviceCollectionMock = new ServiceCollectionMock();

            // When
            SystemWatcherExtensions.AddSystemWatcher(
                serviceCollectionMock
            );

            // Then
            Assert.Collection(
                serviceCollectionMock,
                service =>
                {
                    Assert.Equal(typeof(FileSystemWatcherState), service.ServiceType);
                    Assert.Equal(typeof(InMemoryFileSystemWatcherState), service.ImplementationType);
                },
                service =>
                {
                    Assert.Equal(typeof(PendingReloadState), service.ServiceType);
                    Assert.Equal(typeof(InMemoryPendingReloadState), service.ImplementationType);
                },
                service =>
                {
                    Assert.Equal(typeof(ITimerTask), service.ServiceType);
                    Assert.Equal(typeof(WatchForSystemReloadTimer), service.ImplementationType);
                }
            );
        }

        [Fact]
        public void TestShouldConfigurationApplicationBuilder()
        {
            // Given
            var mocks = ApplicationBuilderFactory.CreateApplicationBuilder();
            var expected = mocks.ApplicationBuilderMock.Object;

            // When
            var actual = SystemWatcherExtensions.UseSystemWatcher(
                mocks.ApplicationBuilderMock.Object
            );

            // Then
            Assert.Equal(
                expected,
                actual
            );
        }
    }
}
