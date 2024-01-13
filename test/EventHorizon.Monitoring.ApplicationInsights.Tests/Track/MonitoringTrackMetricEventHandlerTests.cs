namespace EventHorizon.Monitoring.ApplicationInsights.Tests.Track;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using EventHorizon.Monitoring.ApplicationInsights.Tests.Utils;
using EventHorizon.Monitoring.ApplicationInsights.Track;

using FluentAssertions;

using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

using Xunit;

public class MonitoringTrackMetricEventHandlerTests
{
    [Fact]
    public async Task ShouldTrackMetricOnClientWithNameAndAverage()
    {
        // Given
        var name = "name";
        var average = 123d;

        var telemetryChannelMock = new TelemetryChannelMock();

        var configuration = new TelemetryConfiguration
        {
            TelemetryChannel = telemetryChannelMock,
            InstrumentationKey = Guid.NewGuid().ToString()
        };

        var telemetryClient = new TelemetryClient(
            configuration
        );

        // When
        var handler = new MonitoringTrackMetricEventHandler(
            telemetryClient
        );
        await handler.Handle(
            new Events.Track.MonitoringTrackMetricEvent(
                name,
                average
            ),
            CancellationToken.None
        );

        // Then
        telemetryChannelMock.Items.Cast<MetricTelemetry>()
            .Should().Contain(
                a => a.Name == name
                    && a.Sum == average
            );
    }
}
