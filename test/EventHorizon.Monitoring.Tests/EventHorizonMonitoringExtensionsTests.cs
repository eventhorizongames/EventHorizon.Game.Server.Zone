namespace EventHorizon.Monitoring.Tests;

using System;

using EventHorizon.Monitoring.Model;
using EventHorizon.Test.Common.Utils;

using FluentAssertions;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;

using Moq;

using Xunit;

public class EventHorizonMonitoringExtensionsTests
{
    [Fact]
    public void ShouldReturnRegisteredServicesWhenCalled()
    {
        // Given
        static void options(MonitoringServerConfiguration options) { }

        var serviceCollectionMock = new ServiceCollectionMock();

        // When
        var actual = EventHorizonMonitoringExtensions.AddEventHorizonMonitoring(
            serviceCollectionMock,
            options
        );

        // Then
        Assert.NotEmpty(
            actual
        );
        Assert.Contains(
            actual,
            service => service.ServiceType == typeof(IConfigureOptions<MonitoringServerConfiguration>)
        );
    }

    [Fact]
    public void ShouldAllowForChainingOfCalls()
    {
        // Given
        var applicationBuilderMock = new Mock<IApplicationBuilder>();
        var expected = applicationBuilderMock.Object;

        // When
        var actual = EventHorizonMonitoringExtensions.UseEventHorizonMonitoring(
            applicationBuilderMock.Object
        );

        // Then
        actual.Should().Be(expected);
    }
}
