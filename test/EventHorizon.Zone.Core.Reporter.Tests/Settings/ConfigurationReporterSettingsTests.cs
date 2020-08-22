﻿namespace EventHorizon.Zone.Core.Reporter.Tests.Settings
{
    using System.Collections.Generic;
    using EventHorizon.Zone.Core.Reporter.Settings;
    using FluentAssertions;
    using Microsoft.Extensions.Configuration;
    using Xunit;

    public class ConfigurationReporterSettingsTests
    {
        [Fact]
        public void ShouldSetIsEnabledBasedOnConfigurationWhenCreated()
        {
            // Given
            var elasticsearchUrl = "https://the.world";
            var expectedElasticsearchUrl = elasticsearchUrl;
            var configurationMock = new ConfigurationBuilder()
                .AddInMemoryCollection(
                    new Dictionary<string, string>
                    {
                        { "Reporter:IsEnabled", "true" },
                        { "Reporter:IsWriteToFileEnabled", "false" },
                        { "Reporter:Elasticsearch:IsEnabled", "true" },
                        { "Reporter:Elasticsearch:Url", elasticsearchUrl }
                    }
                )
                .Build();

            // When
            var actual = new ReporterSettingsByConfiguration(
                configurationMock
            );

            // Then
            actual.IsEnabled
                .Should().BeTrue();
            actual.IsWriteToFileEnabled
                .Should().BeFalse();
            actual.Elasticsearch.IsEnabled
                .Should().BeTrue();
            actual.Elasticsearch.Url
                .Should().Be(expectedElasticsearchUrl);
        }

        [Fact]
        public void ShouldResetValueWhenTokenChangeIsTriggered()
        {
            // Given
            var initialElasticsearchUrl = "https://the.world";
            var expectedElasticsearchUrl = "https://the.other.world";
            var configurationMock = new ConfigurationBuilder()
                .AddInMemoryCollection(
                    new Dictionary<string, string>
                    {
                        { "Reporter:IsEnabled", "true" },
                        { "Reporter:IsWriteToFileEnabled", "false" },
                        { "Reporter:Elasticsearch:IsEnabled", "true" },
                        { "Reporter:Elasticsearch:Url", initialElasticsearchUrl }
                    }
                )
                .Build();

            // When
            var actual = new ReporterSettingsByConfiguration(
                configurationMock
            );
            actual.IsEnabled
                .Should().BeTrue();
            actual.Elasticsearch
                .IsEnabled.Should().BeTrue();
            actual.Elasticsearch
                .Url.Should().Be(initialElasticsearchUrl);

            // Update the Configuration Value, triggering change
            configurationMock["Reporter:IsEnabled"] = "false";
            configurationMock["Reporter:Elasticsearch:IsEnabled"] = "false";
            configurationMock["Reporter:Elasticsearch:Url"] = expectedElasticsearchUrl;
            configurationMock.Reload();

            // Then
            actual.IsEnabled
                .Should().BeFalse();
            actual.Elasticsearch
                .IsEnabled.Should().BeFalse();
            actual.Elasticsearch
                .Url.Should().Be(expectedElasticsearchUrl);
        }
    }
}
