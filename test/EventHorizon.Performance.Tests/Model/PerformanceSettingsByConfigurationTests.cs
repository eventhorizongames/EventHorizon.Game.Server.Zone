namespace EventHorizon.Performance.Tests.Model;

using System.Collections.Generic;

using EventHorizon.Performance.Model;

using FluentAssertions;

using Microsoft.Extensions.Configuration;

using Xunit;

public class PerformanceSettingsByConfigurationTests
{
    [Fact]
    public void ShouldSetIsEnabledBasedOnConfigurationWhenCreated()
    {
        // Given
        var configurationMock = new ConfigurationBuilder()
            .AddInMemoryCollection(
                new Dictionary<string, string>
                {
                    { "Performance:IsEnabled", "true" }
                }
            )
            .Build();

        // When
        var actual = new PerformanceSettingsByConfiguration(
            configurationMock
        );

        // Then
        actual.IsEnabled
            .Should().BeTrue();
    }

    [Fact]
    public void ShouldResetValueWhenTokenChangeIsTriggered()
    {
        // Given
        var configurationMock = new ConfigurationBuilder()
            .AddInMemoryCollection(
                new Dictionary<string, string>
                {
                    { "Performance:IsEnabled", "true" }
                }
            )
            .Build();

        // When
        var actual = new PerformanceSettingsByConfiguration(
            configurationMock
        );
        actual.IsEnabled
            .Should().BeTrue();

        // Update the Configuration Value, triggering change
        configurationMock["Performance:IsEnabled"] = "false";
        configurationMock.Reload();

        // Then
        actual.IsEnabled
            .Should().BeFalse();
    }
}
