using Xunit;
using EventHorizon.TimerService;
using EventHorizon.Tests.TestUtils;
using EventHorizon.Game.Server.Zone;
using EventHorizon.Zone.Core.Reporter.Model;
using EventHorizon.Zone.Core.Reporter.Tracker;
using EventHorizon.Zone.Core.Reporter.Timer;

namespace EventHorizon.Zone.Core.Reporter.Tests
{
    public class CoreReporterExtensionsTests
    {
        [Fact]
        public void TestAddCoreReporter_ShouldConfigureServiceCollection()
        {
            // Given
            var serviceCollectionMock = new ServiceCollectionMock();

            // When
            CoreReporterExtensions.AddCoreReporter(
                serviceCollectionMock
            );

            // Then
            Assert.Collection(
                serviceCollectionMock,
                service =>
                {
                    Assert.Equal(typeof(ReportTracker), service.ServiceType);
                    Assert.Equal(typeof(ToRepositoryReportTracker), service.ImplementationInstance.GetType());
                },
                service =>
                {
                    Assert.Equal(typeof(ReportRepository), service.ServiceType);
                    Assert.Equal(typeof(ToRepositoryReportTracker), service.ImplementationInstance.GetType());
                },
                service =>
                {
                    Assert.Equal(typeof(ITimerTask), service.ServiceType);
                    Assert.Equal(typeof(SavePendingReportItemsTimer), service.ImplementationType);
                }
            );
        }
    }
}