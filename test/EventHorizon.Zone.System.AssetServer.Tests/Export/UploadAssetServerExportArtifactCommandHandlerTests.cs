namespace EventHorizon.Zone.System.AssetServer.Tests.Export;

using AutoFixture.Xunit2;

using EventHorizon.Test.Common.Attributes;
using EventHorizon.Zone.Core.Model.Command;
using EventHorizon.Zone.System;

using global::System.IO;
using global::System.Threading.Tasks;
using global::System.Threading;
using MediatR;
using Moq;
using Xunit;
using EventHorizon.Zone.System.AssetServer.Export;
using EventHorizon.Zone.System.AssetServer.Base;
using FluentAssertions;
using EventHorizon.Zone.System.AssetServer.Model;

public class UploadAssetServerExportArtifactCommandHandlerTests
{
    [Theory, AutoMoqData]
    public async Task ReturnServerAndPathCombinationWhenUploadCommandIsSuccessful(
        // Given
        string service,
        string fileFullName,
        string backupPath,
        [Frozen] AssetServerSystemSettings assetServerSystemSettings,
        [Frozen] Mock<ISender> senderMock,
        UploadAssetServerExportArtifactCommandHandler handler
    )
    {
        using var content = new MemoryStream();
        var expected = $"{assetServerSystemSettings.Server}{backupPath}";
        var url = $"{assetServerSystemSettings.Server}/api/Export/{service}/Upload";
        var result = new UploadAssetServerArtifactResult
        {
            Service = service,
            Path = backupPath,
        };

        senderMock.Setup(
            mock => mock.Send(
                new UploadFileToAssetServerCommand(
                    "Export",
                    url,
                    fileFullName,
                    service,
                    content
                ),
                It.IsAny<CancellationToken>()
            )
        ).ReturnsAsync(
            result.ToCommandResult()
        );

        // When
        var actual = await handler.Handle(
            new UploadAssetServerExportArtifactCommand(
                service,
                fileFullName,
                content
            ),
            CancellationToken.None
        );

        // Then
        actual.Result.Service.Should().Be(service);
        actual.Result.Url.Should().Be(expected);
    }

    [Theory, AutoMoqData]
    public async Task ReturnErrorCodeFromUploadCommandWhenResultIsNotSuccessful(
        // Given
        string errorCode,
        string service,
        string fileFullName,
        [Frozen] Mock<ISender> senderMock,
        UploadAssetServerExportArtifactCommandHandler handler
    )
    {
        using var content = new MemoryStream();

        senderMock.Setup(
            mock => mock.Send(
                It.IsAny<UploadFileToAssetServerCommand>(),
                It.IsAny<CancellationToken>()
            )
        ).ReturnsAsync(
            new CommandResult<UploadAssetServerArtifactResult>(
                errorCode
            )
        );

        // When
        var actual = await handler.Handle(
            new UploadAssetServerExportArtifactCommand(
                service,
                fileFullName,
                content
            ),
            CancellationToken.None
        );

        // Then
        actual.Success.Should().BeFalse();
        actual.ErrorCode.Should().Be(errorCode);
    }
}
