namespace EventHorizon.Zone.System.Backup.Tests.Create;

using AutoFixture.Xunit2;

using EventHorizon.Test.Common.Attributes;
using EventHorizon.Zone.Core.Model.DateTimeService;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.Core.Model.Json;
using EventHorizon.Zone.System.Backup.Create;
using EventHorizon.Zone.System.Backup.Events;
using EventHorizon.Zone.System.Backup.Model;

using FluentAssertions;

using global::System;
using global::System.Threading;
using global::System.Threading.Tasks;

using Moq;

using Xunit;

public class CreateBackupOfFileContentCommandHandlerTests
{
    private static DateTime JAVASCRIPT_OFFSET => new(
        1970,
        1,
        1,
        0,
        0,
        0,
        DateTimeKind.Utc
    );

    [Theory, AutoMoqData]
    public async Task ShouldSendCreateBackupOfContentCommandWhenValidFileFullNameIsHandled(
        // Given
        string systemBackupPath,
        string fileName,
        string fileContent,
        string[] filePath,
        [Frozen] Mock<ServerInfo> serverInfoMock,
        [Frozen] Mock<IDateTimeService> dateTimeServiceMock,
        [Frozen] Mock<IJsonFileSaver> fileSaverMock,
        CreateBackupOfFileContentCommandHandler handler
    )
    {
        var created = dateTimeServiceMock.Object.Now;
        var createdString = created
            .Subtract(
                JAVASCRIPT_OFFSET
            ).TotalMilliseconds
            .ToString();
        var backupFileName = string.Join(
            "_",
            filePath
        ) + "_" + fileName + "__" + createdString + "__.json";
        var request = new CreateBackupOfFileContentCommand(
            filePath,
            fileName,
            fileContent
        );
        var backupFile = new BackupFileData(
            fileName,
            filePath,
            fileContent,
            created
        );
        var expected = new BackupFileResponse(
            backupFile
        );

        serverInfoMock.Setup(
            mock => mock.SystemBackupPath
        ).Returns(
            systemBackupPath
        );

        // When
        var actual = await handler.Handle(
            request,
            CancellationToken.None
        );

        // Then
        actual.Should().BeEquivalentTo(expected);

        fileSaverMock.Verify(
            mock => mock.SaveToFile(
                systemBackupPath,
                backupFileName,
                backupFile
            )
        );
    }
}
