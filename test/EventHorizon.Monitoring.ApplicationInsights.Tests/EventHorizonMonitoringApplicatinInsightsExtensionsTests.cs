namespace EventHorizon.Monitoring.ApplicationInsights.Tests
{
    using System;
    using EventHorizon.Monitoring.ApplicationInsights.Telemetry;
    using EventHorizon.Test.Common.Utils;
    using FluentAssertions;
    using Microsoft.ApplicationInsights.AspNetCore.Extensions;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.AspNetCore.Builder;
    using Moq;
    using Xunit;

    public class EventHorizonMonitoringApplicatinInsightsExtensionsTests
    {
        [Fact]
        public void ShouldReturnRegisteredServicesWhenCalled()
        {
            // Given
            Action<ApplicationInsightsServiceOptions> options = options => { };
            var serviceCollectionMock = new ServiceCollectionMock();

            // When
            var actual = EventHorizonMonitoringApplicatinInsightsExtensions.AddEventHorizonMonitoringApplicationInsights(
                serviceCollectionMock,
                options
            );

            // Then
            Assert.NotEmpty(
                actual
            );
            Assert.Contains(
                actual,
                service => service.ServiceType == typeof(ITelemetryInitializer)
                    && service.ImplementationType == typeof(NodeNameFilter)
            );
        }

        [Fact]
        public void ShouldAllowForChainingOfCalls()
        {
            // Given
            var applicationBuilderMock = new Mock<IApplicationBuilder>();
            var expected = applicationBuilderMock.Object;

            // When
            var actual = EventHorizonMonitoringApplicatinInsightsExtensions.UseEventHorizonMonitoringApplicatinInsights(
                applicationBuilderMock.Object
            );

            // Then
            actual.Should().Be(expected);
        }
    }
}
