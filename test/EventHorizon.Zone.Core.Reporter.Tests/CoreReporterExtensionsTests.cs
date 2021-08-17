namespace EventHorizon.Zone.Core.Reporter.Tests
{
    using System;
    using System.Threading;

    using EventHorizon.Game.Server.Zone;
    using EventHorizon.Test.Common;
    using EventHorizon.Test.Common.Utils;
    using EventHorizon.TimerService;
    using EventHorizon.Zone.Core.Model.DateTimeService;
    using EventHorizon.Zone.Core.Reporter.Model;
    using EventHorizon.Zone.Core.Reporter.Settings;
    using EventHorizon.Zone.Core.Reporter.Timer;
    using EventHorizon.Zone.Core.Reporter.Tracker;
    using EventHorizon.Zone.Core.Reporter.Writer.Client;
    using EventHorizon.Zone.Core.Reporter.Writer.Client.Startup;
    using EventHorizon.Zone.Core.Reporter.Writer.Client.Timer;

    using MediatR;

    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    using Moq;

    using Xunit;

    public class CoreReporterExtensionsTests
    {
        [Fact]
        public void ShouldConfigureServiceCollectionWhenAddCoreReporterIsCalled()
        {
            // Given
            var serviceCollectionMock = new ServiceCollectionMock();

            var serviceProviderMock = new Mock<IServiceProvider>();
            var dateTimeServiceMock = new Mock<IDateTimeService>();
            var reporterSettingsMock = new Mock<ReporterSettings>();
            var reportTrackingRepositoryMock = new Mock<ReportTrackingRepository>(
                dateTimeServiceMock.Object
            );
            var reportTrackingRepositoryBySettingsMock = new Mock<ReportTrackingRepositoryBySettings>(
                reportTrackingRepositoryMock.Object,
                reporterSettingsMock.Object
            );
            var elasticsearchReporterClientBasedOnElasticClientMock = new Mock<ElasticsearchReporterClientBasedOnElasticClient>(
                It.IsAny<ILogger<ElasticsearchReporterClientBasedOnElasticClient>>(),
                reporterSettingsMock.Object
            );

            serviceProviderMock.Setup(
                mock => mock.GetService(
                    typeof(ReportTrackingRepository)
                )
            ).Returns(
                reportTrackingRepositoryMock.Object
            );
            serviceProviderMock.Setup(
                mock => mock.GetService(
                    typeof(ReportTrackingRepositoryBySettings)
                )
            ).Returns(
                reportTrackingRepositoryBySettingsMock.Object
            );

            serviceProviderMock.Setup(
                mock => mock.GetService(
                    typeof(ElasticsearchReporterClientBasedOnElasticClient)
                )
            ).Returns(
                elasticsearchReporterClientBasedOnElasticClientMock.Object
            );

            // When
            CoreReporterExtensions.AddCoreReporter(
                serviceCollectionMock
            );

            // Then
            Assert.Contains(
                serviceCollectionMock,
                service => service.ServiceType == typeof(ITimerTask)
                    && service.ImplementationType == typeof(CheckElasticsearchReporterClientConnectionTimerTask)
            );
            Assert.Contains(
                serviceCollectionMock,
                service => service.ServiceType == typeof(ElasticsearchReporterClientBasedOnElasticClient)
                    && service.ImplementationType == typeof(ElasticsearchReporterClientBasedOnElasticClient)
            );
            Assert.Contains(
                serviceCollectionMock,
                service =>
                    service.ServiceType == typeof(ElasticsearchReporterClient)
                    && service.ImplementationFactory(serviceProviderMock.Object) is ElasticsearchReporterClientBasedOnElasticClient
            );
            Assert.Contains(
                serviceCollectionMock,
                service =>
                    service.ServiceType == typeof(ElasticsearchReporterClientStartup)
                    && service.ImplementationFactory(serviceProviderMock.Object) is ElasticsearchReporterClientBasedOnElasticClient
            );
            Assert.Contains(
                serviceCollectionMock,
                service => service.ServiceType == typeof(ReportTrackingRepository)
                    && service.ImplementationType == typeof(ReportTrackingRepository)
            );
            Assert.Contains(
                serviceCollectionMock,
                service => service.ServiceType == typeof(ReportTrackingRepositoryBySettings)
                    && service.ImplementationType == typeof(ReportTrackingRepositoryBySettings)
            );
            Assert.Contains(
                serviceCollectionMock,
                service => service.ServiceType == typeof(ReporterSettings)
                    && service.ImplementationType == typeof(ReporterSettingsByConfiguration)
            );
            Assert.Contains(
                serviceCollectionMock,
                service =>
                    service.ServiceType == typeof(ReportTracker)
                    && service.ImplementationFactory(serviceProviderMock.Object) is ReportTrackingRepositoryBySettings
            );
            Assert.Contains(
                serviceCollectionMock,
                service =>
                    service.ServiceType == typeof(ReportRepository)
                    && service.ImplementationFactory(serviceProviderMock.Object) is ReportTrackingRepositoryBySettings
            );
            Assert.Contains(
                serviceCollectionMock,
                service => typeof(ITimerTask) == service.ServiceType
                    && service.ImplementationType == typeof(SavePendingReportItemsTimer)
            );
        }

        [Fact]
        public void TestShouldPublishFillServerPropertiesEventWhenUseCoreIsCalled()
        {
            // Given
            var applicationBuilderMocks = ApplicationBuilderFactory.CreateApplicationBuilder();
            var expected = new StartupElasticsearchReporterClient();

            var mediatorMock = new Mock<IMediator>();

            applicationBuilderMocks.ServiceProviderMock.Setup(
                mock => mock.GetService(typeof(IMediator))
            ).Returns(
                mediatorMock.Object
            );

            // When
            var actual = CoreReporterExtensions.UseCoreReporter(
                applicationBuilderMocks.ApplicationBuilderMock.Object
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Send(
                    new StartupElasticsearchReporterClient(),
                    CancellationToken.None
                )
            );
        }
    }
}
