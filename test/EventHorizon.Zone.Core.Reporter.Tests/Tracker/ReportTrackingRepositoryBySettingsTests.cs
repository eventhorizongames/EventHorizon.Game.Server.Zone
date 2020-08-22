namespace EventHorizon.Zone.Core.Reporter.Tests.Tracker
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using Castle.DynamicProxy.Generators;
    using EventHorizon.Zone.Core.Model.DateTimeService;
    using EventHorizon.Zone.Core.Reporter.Model;
    using EventHorizon.Zone.Core.Reporter.Tracker;
    using FluentAssertions;
    using Moq;
    using Xunit;

    public class ReportTrackingRepositoryBySettingsTests
    {
        [Fact]
        public void ShouldCallClearOnTrackingRepositoryWhenSettingsIsEnabledIsTrue()
        {
            // Given
            var reportId = "report-id";

            var dateTimeServiceMock = new Mock<IDateTimeService>();
            var reportTrackingRepostioryMock = new Mock<ReportTrackingRepository>(
                dateTimeServiceMock.Object
            );
            var reporterSettingsMock = new Mock<ReporterSettings>();

            reporterSettingsMock.Setup(
                mock => mock.IsEnabled
            ).Returns(
                true
            );

            // When
            var repository = new ReportTrackingRepositoryBySettings(
                reportTrackingRepostioryMock.Object,
                reporterSettingsMock.Object
            );

            repository.Clear(
                reportId
            );

            // Then
            reportTrackingRepostioryMock.Verify(
                mock => mock.Clear(
                    reportId
                )
            );
        }

        [Fact]
        public void ShouldNotCallClearOnTrackingRepositoryWhenSettingIsEnabledIsFalse()
        {
            // Given
            var reportId = "report-id";

            var dateTimeServiceMock = new Mock<IDateTimeService>();
            var reportTrackingRepostioryMock = new Mock<ReportTrackingRepository>(
                dateTimeServiceMock.Object
            );
            var reporterSettingsMock = new Mock<ReporterSettings>();

            reporterSettingsMock.Setup(
                mock => mock.IsEnabled
            ).Returns(
                false
            );

            // When
            var repository = new ReportTrackingRepositoryBySettings(
                reportTrackingRepostioryMock.Object,
                reporterSettingsMock.Object
            );

            repository.Clear(
                reportId
            );

            // Then
            reportTrackingRepostioryMock.Verify(
                mock => mock.Clear(
                    It.IsAny<string>()
                ),
                Times.Never()
            );
        }

        [Fact]
        public void ShouldCallTakeAllOnTrackingRepositoryWhenSettingIsEnabledIsTrue()
        {
            // Given
            var expected = new List<Report>
            {
                new Report(),
            };

            var dateTimeServiceMock = new Mock<IDateTimeService>();
            var reportTrackingRepostioryMock = new Mock<ReportTrackingRepository>(
                dateTimeServiceMock.Object
            );
            var reporterSettingsMock = new Mock<ReporterSettings>();

            reporterSettingsMock.Setup(
                mock => mock.IsEnabled
            ).Returns(
                true
            );

            reportTrackingRepostioryMock.Setup(
                mock => mock.TakeAll()
            ).Returns(
                expected
            );

            // When
            var repository = new ReportTrackingRepositoryBySettings(
                reportTrackingRepostioryMock.Object,
                reporterSettingsMock.Object
            );

            var actual = repository.TakeAll();

            // Then
            actual.Should().BeEquivalentTo(expected);
            reportTrackingRepostioryMock.Verify(
                mock => mock.TakeAll()
            );
        }

        [Fact]
        public void ShouldNotCallClearOnTrackingRepositoryWhenSettingIsNotEnabled()
        {
            // Given
            var expected = new List<Report>();

            var dateTimeServiceMock = new Mock<IDateTimeService>();
            var reportTrackingRepostioryMock = new Mock<ReportTrackingRepository>(
                dateTimeServiceMock.Object
            );
            var reporterSettingsMock = new Mock<ReporterSettings>();

            reporterSettingsMock.Setup(
                mock => mock.IsEnabled
            ).Returns(
                false
            );

            reportTrackingRepostioryMock.Setup(
                mock => mock.TakeAll()
            ).Returns(
                new List<Report>
                {
                    new Report()
                }
            );

            // When
            var repository = new ReportTrackingRepositoryBySettings(
                reportTrackingRepostioryMock.Object,
                reporterSettingsMock.Object
            );

            var actual = repository.TakeAll();

            // Then
            actual.Should().BeEquivalentTo(expected);
            reportTrackingRepostioryMock.Verify(
                mock => mock.TakeAll(),
                Times.Never()
            );
        }

        [Fact]
        public void ShouldCallTrackOnTrackingRepositoryWhenSettingIsEnabledIsTrue()
        {
            // Given
            var id = "id";
            var correlationId = "correlation-id";
            var message = "message";
            var data = new { };

            var expectedId = id;
            var expectedCorrelationId = correlationId;
            var expectedMessage = message;
            var expectedData = data;

            var dateTimeServiceMock = new Mock<IDateTimeService>();
            var reportTrackingRepostioryMock = new Mock<ReportTrackingRepository>(
                dateTimeServiceMock.Object
            );
            var reporterSettingsMock = new Mock<ReporterSettings>();

            reporterSettingsMock.Setup(
                mock => mock.IsEnabled
            ).Returns(
                true
            );

            // When
            var repository = new ReportTrackingRepositoryBySettings(
                reportTrackingRepostioryMock.Object,
                reporterSettingsMock.Object
            );

            repository.Track(
                id,
                correlationId,
                message,
                data
            );

            // Then
            reportTrackingRepostioryMock.Verify(
                mock => mock.Track(
                    expectedId,
                    expectedCorrelationId,
                    expectedMessage,
                    expectedData
                )
            );
        }

        [Fact]
        public void ShouldNotCallTrackOnTrackingRepositoryWhenSettingIsNotEnabled()
        {
            // Given
            var id = "id";
            var correlationId = "correlation-id";
            var message = "message";
            var data = new { };

            var dateTimeServiceMock = new Mock<IDateTimeService>();
            var reportTrackingRepostioryMock = new Mock<ReportTrackingRepository>(
                dateTimeServiceMock.Object
            );
            var reporterSettingsMock = new Mock<ReporterSettings>();

            reporterSettingsMock.Setup(
                mock => mock.IsEnabled
            ).Returns(
                false
            );

            // When
            var repository = new ReportTrackingRepositoryBySettings(
                reportTrackingRepostioryMock.Object,
                reporterSettingsMock.Object
            );

            repository.Track(
                id,
                correlationId,
                message,
                data
            );

            // Then
            reportTrackingRepostioryMock.Verify(
                mock => mock.Track(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<object>()
                ),
                Times.Never()
            );
        }
    }
}
