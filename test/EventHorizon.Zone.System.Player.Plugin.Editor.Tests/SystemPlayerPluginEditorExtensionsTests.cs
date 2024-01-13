﻿namespace EventHorizon.Zone.System.Player.Plugin.Editor.Tests;

using EventHorizon.Game.Server.Zone;
using EventHorizon.Test.Common;
using EventHorizon.Test.Common.Utils;

using FluentAssertions;

using Xunit;

public class SystemPlayerPluginEditorExtensionsTests
{
    [Fact]
    public void ShouldReturnPassedInCall()
    {
        // Given
        var serviceCollectionMock = new ServiceCollectionMock();
        var expected = serviceCollectionMock;

        // When
        var actual = SystemPlayerPluginEditorExtensions.AddSystemPlayerPluginEditor(
            serviceCollectionMock
        );

        // Then
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void ShouldReturnPassedIn()
    {
        // Given
        var applicationBuilder = ApplicationBuilderFactory.CreateApplicationBuilder();
        var expected = applicationBuilder.ApplicationBuilderMock.Object;

        // When
        var actual = SystemPlayerPluginEditorExtensions.UseSystemPlayerPluginEditor(
            applicationBuilder.ApplicationBuilderMock.Object
        );

        // Then
        actual.Should().Be(expected);
    }
}
