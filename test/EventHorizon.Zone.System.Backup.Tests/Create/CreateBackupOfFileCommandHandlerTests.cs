namespace EventHorizon.Zone.System.Backup.Tests.Create
{
    using AutoFixture.Xunit2;

    using EventHorizon.Test.Common.Attributes;
    using EventHorizon.Zone.Core.Events.FileService;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.System.Backup.Create;
    using EventHorizon.Zone.System.Backup.Events;
    using EventHorizon.Zone.System.Backup.Model;

    using FluentAssertions;

    using global::System.IO;
    using global::System.Linq;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    using Moq;

    using Xunit;

    public class CreateBackupOfFileCommandHandlerTests
    {
        [Theory, AutoMoqData]
        public async Task ShouldSendCreateBackupOfContentCommandWhenValidFileFullNameIsHandled(
            // Given
            BackupFileResponse expected,
            [Frozen] Mock<IMediator> mediatorMock,
            [Frozen] Mock<ServerInfo> serverInfoMock,
            CreateBackupOfFileCommandHandler handler
        )
        {
            var appDataPath = $"{Path.DirectorySeparatorChar}app-data-path";
            var fileName = "file-name.exe";
            var fileContent = "file-contents";
            var childOfPath = "child-of-path";
            var filePath = new string[] { childOfPath };
            var fileFullName = Path.Combine(
                appDataPath,
                childOfPath,
                fileName
            );
            var request = new CreateBackupOfFileCommand(
                fileFullName
            );


            mediatorMock.Setup(
                mock => mock.Send(
                    It.Is<CreateBackupOfFileContentCommand>(
                        a => a.FilePath.Contains(filePath.First())
                            && a.FileName == fileName
                            && a.FileContent == fileContent
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                expected
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new ReadAllTextFromFile(
                        fileFullName
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                fileContent
            );

            serverInfoMock.Setup(
                mock => mock.AppDataPath
            ).Returns(
                appDataPath
            );

            // When
            var actual = await handler.Handle(
                request,
                CancellationToken.None
            );

            // Then
            actual.Should().BeEquivalentTo(expected);
        }
    }
}
