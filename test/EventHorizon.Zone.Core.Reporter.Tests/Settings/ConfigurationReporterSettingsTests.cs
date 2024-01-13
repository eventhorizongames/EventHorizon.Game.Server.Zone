namespace EventHorizon.Zone.Core.Reporter.Tests.Settings;

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
        var elasticsearchUri = "https://the.world";
        var elasticsearchUsername = "es_username";
        var elasticsearchPassword = "es_password";
        var expectedElasticsearchUri = elasticsearchUri;
        var expectedElasticsearchUsername = elasticsearchUsername;
        var expectedElasticsearchPassword = elasticsearchPassword;
        var configurationMock = new ConfigurationBuilder()
            .AddInMemoryCollection(
                new Dictionary<string, string>
                {
                    { "Reporter:IsEnabled", "true" },
                    { "Reporter:IsWriteToFileEnabled", "false" },
                    { "Reporter:Elasticsearch:IsEnabled", "true" },
                    { "Elasticsearch:Uri", elasticsearchUri },
                    { "Elasticsearch:Username", elasticsearchUsername },
                    { "Elasticsearch:Password", elasticsearchPassword },
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
        actual.Elasticsearch.Uri
            .Should().Be(expectedElasticsearchUri);
        actual.Elasticsearch.Username
            .Should().Be(expectedElasticsearchUsername);
        actual.Elasticsearch.Password
            .Should().Be(expectedElasticsearchPassword);
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
                    { "Elasticsearch:Uri", initialElasticsearchUrl },
                    { "Elasticsearch:Username", "username" },
                    { "Elasticsearch:Password", "password" },
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
            .Uri.Should().Be(initialElasticsearchUrl);

        // Update the Configuration Value, triggering change
        configurationMock["Reporter:IsEnabled"] = "false";
        configurationMock["Reporter:Elasticsearch:IsEnabled"] = "false";
        configurationMock["Elasticsearch:Uri"] = expectedElasticsearchUrl;
        configurationMock.Reload();

        // Then
        actual.IsEnabled
            .Should().BeFalse();
        actual.Elasticsearch
            .IsEnabled.Should().BeFalse();
        actual.Elasticsearch
            .Uri.Should().Be(expectedElasticsearchUrl);
    }
}
