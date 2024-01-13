namespace EventHorizon.Monitoring.ApplicationInsights.Tests.Telemetry;

using EventHorizon.Monitoring.ApplicationInsights.Telemetry;
using EventHorizon.Monitoring.Model;

using FluentAssertions;

using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using Microsoft.Extensions.Options;

using Moq;

using Xunit;

public class NodeNameFilterTests
{
    [Theory]
    [InlineData(null, "ServerName", "Host", "ServerName (Host)")]
    [InlineData("", "ServerName", "Host", "ServerName (Host)")]
    [InlineData("Already Set Value", "ServerName", "Host", "Already Set Value")]
    public void ShouldSetInternalContextNodeNameWhenNodeNameIsNull(
        string nodeName,
        string serverName,
        string host,
        string expected
    )
    {
        // Given
        var serverConfig = new MonitoringServerConfiguration()
        {
            ServerName = serverName,
            Host = host,
        };

        var telemetryContext = new TelemetryContext();
        telemetryContext.GetInternalContext().NodeName = nodeName;

        var optionsMock = new Mock<IOptions<MonitoringServerConfiguration>>();
        var telemetryMock = new Mock<ITelemetry>();

        optionsMock.Setup(
            mock => mock.Value
        ).Returns(
            serverConfig
        );

        telemetryMock.Setup(
            mock => mock.Context
        ).Returns(
            telemetryContext
        );

        // When
        var filter = new NodeNameFilter(
            optionsMock.Object
        );
        filter.Initialize(
            telemetryMock.Object
        );

        var actual = telemetryContext.GetInternalContext();

        // Then
        actual.NodeName.Should().Be(expected);
    }

    [Fact]
    public void ShouldSetHostOnGlobalPropertiesWhenNotAlreadyContainingKey(
    )
    {
        // Given
        var input = "https://host-url";
        var expected = input;
        var expectedGlobalPropertyKey = "HOST";
        var serverConfig = new MonitoringServerConfiguration()
        {
            ServerName = "serverName",
            Host = input,
        };

        var telemetryContext = new TelemetryContext();

        var optionsMock = new Mock<IOptions<MonitoringServerConfiguration>>();
        var telemetryMock = new Mock<ITelemetry>();

        optionsMock.Setup(
            mock => mock.Value
        ).Returns(
            serverConfig
        );

        telemetryMock.Setup(
            mock => mock.Context
        ).Returns(
            telemetryContext
        );

        // When
        var filter = new NodeNameFilter(
            optionsMock.Object
        );
        filter.Initialize(
            telemetryMock.Object
        );

        var actual = telemetryContext;

        // Then
        actual.GlobalProperties
            .Should().ContainKey(expectedGlobalPropertyKey);
        actual.GlobalProperties
            .Should().ContainValue(expected);
    }

    [Fact]
    public void ShouldNotSetHostOnGlobalPropertiesWhenAlreadyContainingKey(
    )
    {
        // Given
        var hostGlobalProperty = "https://host-url";
        var expected = hostGlobalProperty;
        var expectedGlobalPropertyKey = "HOST";
        var serverConfig = new MonitoringServerConfiguration()
        {
            ServerName = "serverName",
            Host = "different-value",
        };

        var telemetryContext = new TelemetryContext();
        telemetryContext.GlobalProperties.Add(
            expectedGlobalPropertyKey,
            hostGlobalProperty
        );

        var optionsMock = new Mock<IOptions<MonitoringServerConfiguration>>();
        var telemetryMock = new Mock<ITelemetry>();

        optionsMock.Setup(
            mock => mock.Value
        ).Returns(
            serverConfig
        );

        telemetryMock.Setup(
            mock => mock.Context
        ).Returns(
            telemetryContext
        );

        // When
        var filter = new NodeNameFilter(
            optionsMock.Object
        );
        filter.Initialize(
            telemetryMock.Object
        );

        var actual = telemetryContext;

        // Then
        actual.GlobalProperties
            .Should().ContainKey(expectedGlobalPropertyKey);
        actual.GlobalProperties
            .Should().ContainValue(expected);
    }
}
