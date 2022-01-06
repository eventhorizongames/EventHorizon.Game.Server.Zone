namespace EventHorizon.Zone.System.AssetServer.Tests.Backup;

using AutoFixture.Xunit2;

using EventHorizon.Test.Common.Attributes;
using EventHorizon.Zone.System;

using global::System.IO;
using global::System.Net.Http;
using global::System.Net;
using global::System.Threading.Tasks;
using global::System.Threading;
using global::System;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq.Protected;
using Moq;
using Xunit;
using EventHorizon.Zone.System.AssetServer.Model;
using EventHorizon.Zone.System.AssetServer.Backup;
using FluentAssertions;
using EventHorizon.Zone.System.AssetServer.Base;
using EventHorizon.Zone.Core.Model.Command;

public class UploadAssetServerBackupArtifactCommandHandlerTests
{
    [Theory, AutoMoqData]
    public async Task ReturnServerAndPathCombinationWhenUploadCommandIsSuccessful(
        // Given
        string service,
        string fileFullName,
        string backupPath,
        [Frozen] AssetServerSystemSettings assetServerSystemSettings,
        [Frozen] Mock<ISender> senderMock,
        UploadAssetServerBackupArtifactCommandHandler handler
    )
    {
        using var content = new MemoryStream();
        var expected = $"{assetServerSystemSettings.Server}{backupPath}";
        var url = $"{assetServerSystemSettings.Server}/api/Backup/{service}/Upload";
        var result = new UploadAssetServerArtifactResult
        {
            Service = service,
            Path = backupPath,
        };

        senderMock.Setup(
            mock => mock.Send(
                new UploadFileToAssetServerCommand(
                    "Backup",
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
            new UploadAssetServerBackupArtifactCommand(
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
        UploadAssetServerBackupArtifactCommandHandler handler
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
            new UploadAssetServerBackupArtifactCommand(
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
