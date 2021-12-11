namespace EventHorizon.Zone.System.ArtifactManagement.Tests.Query;

using AutoFixture.Xunit2;

using EventHorizon.Test.Common.Attributes;
using EventHorizon.Zone.System.ArtifactManagement.Model;
using EventHorizon.Zone.System.ArtifactManagement.Query;

using FluentAssertions;

using global::System.Collections.Generic;
using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

using Moq;

using Xunit;

public class IsNotValidArtifactUrlDomainHandlerTests
{
    [Theory]
    [InlineAutoMoqData("https://invalid-domain.com/file.zip")]
    [InlineAutoMoqData("https://domain/file.zip")]
    [InlineAutoMoqData("folder/file.zip")]
    [InlineAutoMoqData("//folder/file.zip")]
    [InlineAutoMoqData("\\folder/file.zip")]
    [InlineAutoMoqData("C:\\folder\\file.zip")]
    public async Task ReturnTrueWhenImportArtifactUrlIsNotInAllowList(
        // Given
        string artifactUrl,
        [Frozen] Mock<ArtifactManagementSystemSettings> settings,
        IsNotValidArtifactUrlDomainHandler handler
    )
    {
        settings.Setup(
            mock => mock.AllowedDomainList
        ).Returns(
            new List<string>
            {
                "valid-domain.com",
            }
        );

        // When
        var actual = await handler.Handle(
            new IsNotValidArtifactUrlDomain(
                artifactUrl
            ),
            CancellationToken.None
        );

        // Then
        actual.Should().BeTrue();
    }

    [Theory]
    [InlineAutoMoqData("https://domain.com/file.zip")]
    [InlineAutoMoqData("https://domain/file.zip")]
    public async Task ReturnTrueWhenAllowedDomainListIsEmpty(
        // Given
        string importArtifactUrl,
        [Frozen] Mock<ArtifactManagementSystemSettings> settings,
        IsNotValidArtifactUrlDomainHandler handler
    )
    {
        settings.Setup(
            mock => mock.AllowedDomainList
        ).Returns(
            new List<string>()
        );

        // When
        var actual = await handler.Handle(
            new IsNotValidArtifactUrlDomain(
                importArtifactUrl
            ),
            CancellationToken.None
        );

        // Then
        actual.Should().BeTrue();
    }

    [Theory]
    [InlineAutoMoqData("folder/file.zip")]
    [InlineAutoMoqData("/folder/file.zip")]
    [InlineAutoMoqData("\\folder/file.zip")]
    [InlineAutoMoqData("C:\\folder\\file.zip")]
    public async Task ReturnTrueWhenImportArtifactUrlIsNotAUrl(
        // Given
        string importArtifactUrl,
        [Frozen] Mock<ArtifactManagementSystemSettings> settings,
        IsNotValidArtifactUrlDomainHandler handler
    )
    {
        settings.Setup(
            mock => mock.AllowedDomainList
        ).Returns(
            new List<string>
            {
                ArtifactManagementSystemContants.ALL_DOMAINS,
            }
        );

        // When
        var actual = await handler.Handle(
            new IsNotValidArtifactUrlDomain(
                importArtifactUrl
            ),
            CancellationToken.None
        );

        // Then
        actual.Should().BeTrue();
    }

    [Theory]
    [InlineAutoMoqData("//folder/file.zip")]
    [InlineAutoMoqData("//file.zip")]
    [InlineAutoMoqData("//file")]
    [InlineAutoMoqData("https://domain1.com/file.zip")]
    [InlineAutoMoqData("https://domain2.com/file.zip")]
    [InlineAutoMoqData("http://domain3.com/file.zip")]
    [InlineAutoMoqData("ftp://domain3.com/file.zip")]
    [InlineAutoMoqData("ftps://domain3.com/file.zip")]
    [InlineAutoMoqData("http://domain4.com/file.zip")]
    [InlineAutoMoqData("https://domain5.com/file.zip")]
    public async Task ReturnFalseWhenForAnyImportUrlWhenAllDomainsConstantsIsInAllowedDomainList(
        // Given
        string importArtifactUrl,
        [Frozen] Mock<ArtifactManagementSystemSettings> settingsMock,
        IsNotValidArtifactUrlDomainHandler handler
    )
    {
        settingsMock.Setup(
            mock => mock.AllowedDomainList
        ).Returns(
            new List<string>
            {
                ArtifactManagementSystemContants.ALL_DOMAINS,
            }
        );

        // When
        var actual = await handler.Handle(
            new IsNotValidArtifactUrlDomain(
                importArtifactUrl
            ),
            CancellationToken.None
        );

        // Then
        actual.Should().BeFalse();
    }

    [Theory]
    [InlineAutoMoqData("https://valid-domain.com/file.zip")]
    [InlineAutoMoqData("https://validdomain/file.zip")]
    public async Task ReturnFalseWhenImportArtifactUrlIsInAllowList(
        // Given
        string importArtifactUrl,
        [Frozen] Mock<ArtifactManagementSystemSettings> settingsMock,
        IsNotValidArtifactUrlDomainHandler handler
    )
    {
        settingsMock.Setup(
            mock => mock.AllowedDomainList
        ).Returns(
            new List<string>
            {
                "valid-domain.com",
                "validdomain",
            }
        );

        // When
        var actual = await handler.Handle(
            new IsNotValidArtifactUrlDomain(
                importArtifactUrl
            ),
            CancellationToken.None
        );

        // Then
        actual.Should().BeFalse();
    }
}
