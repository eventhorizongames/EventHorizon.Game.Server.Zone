namespace EventHorizon.Server.Core.Connection.Tests.Model;

using EventHorizon.Server.Core.Connection.Model;

using FluentAssertions;

using Xunit;

public class ZoneRegistrationDetailsTests
{
    [Fact]
    public void ShouldHaveValidatePropertiesWhenSet()
    {
        // Given
        var serverAddress = "server-address";
        var tag = "tag";
        var applicationVersion = "application-version";

        var expectedServerAddress = serverAddress;
        var expectedTag = tag;
        var expectedApplicationVersion = applicationVersion;

        // When
        var details = new ZoneRegistrationDetails(
            serverAddress,
            tag,
            new ServiceDetails(
                applicationVersion
            )
        );

        // Then
        details.ServerAddress
            .Should().Be(
                expectedServerAddress
            );
        details.Tag
            .Should().Be(
                expectedTag
            );
        details.Details.ApplicationVersion
            .Should().Be(
                expectedApplicationVersion
            );
    }
}
