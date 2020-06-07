namespace EventHorizon.Zone.Core.Reporter.Tests
{
    using Xunit;
    using EventHorizon.TimerService;
    using EventHorizon.Tests.TestUtils;
    using EventHorizon.Game.Server.Zone;
    using EventHorizon.Zone.Core.Reporter.Model;
    using EventHorizon.Zone.Core.Reporter.Tracker;
    using EventHorizon.Zone.Core.Reporter.Timer;
    using Moq;
    using System;
    using EventHorizon.Zone.Core.Model.DateTimeService;

    public class CoreReporterExtensionsTests
    {
        [Fact]
        public void ShouldConfigureServiceCollectionWhenAddCoreReporterIsCalled()
        {
            // Given
            var serviceCollectionMock = new ServiceCollectionMock();

            var serviceProviderMock = new Mock<IServiceProvider>();
            var dateTimeServiceMock = new Mock<IDateTimeService>();
            var coreServerConnectionFactoryMock = new Mock<ReportTrackingRepository>(
                dateTimeServiceMock.Object
            );

            serviceProviderMock.Setup(
                mock => mock.GetService(
                    typeof(ReportTrackingRepository)
                )
            ).Returns(
                coreServerConnectionFactoryMock.Object
            );

            // When
            CoreReporterExtensions.AddCoreReporter(
                serviceCollectionMock
            );

            // Then
            Assert.Contains(
                serviceCollectionMock,
                service => service.ServiceType == typeof(ReportTrackingRepository)
                    && service.ImplementationType == typeof(ReportTrackingRepository)
            );
            Assert.Contains(
                serviceCollectionMock,
                service =>
                    service.ServiceType == typeof(ReportTracker)
                    && service.ImplementationFactory(serviceProviderMock.Object) is ReportTrackingRepository
            );
            Assert.Contains(
                serviceCollectionMock,
                service =>
                    service.ServiceType == typeof(ReportRepository)
                    && service.ImplementationFactory(serviceProviderMock.Object) is ReportTrackingRepository
            );
            Assert.Contains(
                serviceCollectionMock,
                service => typeof(ITimerTask) == service.ServiceType
                    && service.ImplementationType == typeof(SavePendingReportItemsTimer)
            );
        }
    }
}