namespace EventHorizon.Performance.Tests
{
    using EventHorizon.Performance.Model;
    using EventHorizon.Test.Common.Utils;

    using FluentAssertions;

    using Xunit;

    public class PerformanceExtensionsTests
    {
        [Fact]
        public void ShouldConfigurationServiceCollectionWhenAddPerformanceIsCalled()
        {
            // Given
            var serviceCollectionMock = new ServiceCollectionMock();

            // When
            PerformanceExtensions.AddPerformance(
                serviceCollectionMock
            );

            // Then
            serviceCollectionMock
                .Should()
                .SatisfyRespectively(
                    service =>
                    {
                        Assert.Equal(typeof(PerformanceSettings), service.ServiceType);
                        Assert.Equal(typeof(PerformanceSettingsByConfiguration), service.ImplementationType);
                    },
                    service =>
                    {
                        Assert.Equal(typeof(PerformanceTrackerFactory), service.ServiceType);
                        Assert.Equal(typeof(ToLoggerPerformanceTrackerFactory), service.ImplementationType);
                    }
                );
        }
    }
}
