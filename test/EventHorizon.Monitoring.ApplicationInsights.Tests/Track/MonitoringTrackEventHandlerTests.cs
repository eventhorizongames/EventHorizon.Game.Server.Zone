namespace EventHorizon.Monitoring.ApplicationInsights.Tests.Track;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using EventHorizon.Monitoring.ApplicationInsights.Tests.Utils;
using EventHorizon.Monitoring.ApplicationInsights.Track;
using EventHorizon.Monitoring.Events.Track;

using FluentAssertions;

using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

using Xunit;

public class MonitoringTrackEventHandlerTests
{
    [Fact]
    public async Task ShouldTrackEventOnClientWithName()
    {
        // Given
        var name = "name";

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
        var handler = new MonitoringTrackEventHandler(
            telemetryClient
        );
        await handler.Handle(
            new MonitoringTrackEvent(
                name
            ),
            CancellationToken.None
        );

        // Then
        telemetryChannelMock.Items.Cast<EventTelemetry>()
            .Should().Contain(
                a => a.Name == name
            );
    }
}
