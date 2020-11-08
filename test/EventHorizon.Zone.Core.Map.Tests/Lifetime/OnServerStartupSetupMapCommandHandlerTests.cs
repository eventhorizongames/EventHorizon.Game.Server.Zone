namespace EventHorizon.Zone.Core.Map.Tests.Lifetime
{
    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Zone.Core.Events.FileService;
    using EventHorizon.Zone.Core.Map.Lifetime;
    using EventHorizon.Zone.Core.Map.Model;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.Core.Model.Json;
    using EventHorizon.Zone.Core.Model.Lifetime;
    using FluentAssertions;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    public class OnServerStartupSetupMapCommandHandlerTests
    {
        [Fact]
        public async Task ShouldCreateDefaultSettingsWhenFileDoesNotExist()
        {
            // Given
            var directory = $"{Path.DirectorySeparatorChar}some-file-directory";
            var fileName = "Map.state.json";
            var expected = DefaultMapSettings.DEFAULT_MAP;

            var loggerMock = new Mock<ILogger<OnServerStartupSetupMapCommandHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();
            var fileSaverMock = new Mock<IJsonFileSaver>();

            serverInfoMock.Setup(
                mock => mock.CoreMapPath
            ).Returns(
                directory
            );

            // When
            var handler = new OnServerStartupSetupMapCommandHandler(
                loggerMock.Object,
                mediatorMock.Object,
                serverInfoMock.Object,
                fileSaverMock.Object
            );
            var actual = await handler.Handle(
                new OnServerStartupSetupMapCommand(),
                CancellationToken.None
            );

            // Then
            actual.Should().Be(
                new OnServerStartupResult(
                    true
                )
            );

            fileSaverMock.Verify(
                mock => mock.SaveToFile(
                    directory,
                    fileName,
                    expected
                )
            );
        }

        [Fact]
        public async Task ShouldNotCreateDefaultSettingsWhenFileDoesExist()
        {
            // Given
            var directory = $"{Path.DirectorySeparatorChar}some-file-directory";
            var fileName = "Map.state.json";
            var fileFullName = Path.Combine(
                directory,
                fileName
            );

            var loggerMock = new Mock<ILogger<OnServerStartupSetupMapCommandHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();
            var fileSaverMock = new Mock<IJsonFileSaver>();

            serverInfoMock.Setup(
                mock => mock.CoreMapPath
            ).Returns(
                directory
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new DoesFileExist(
                        fileFullName
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                true
            );

            // When
            var handler = new OnServerStartupSetupMapCommandHandler(
                loggerMock.Object,
                mediatorMock.Object,
                serverInfoMock.Object,
                fileSaverMock.Object
            );
            var actual = await handler.Handle(
                new OnServerStartupSetupMapCommand(),
                CancellationToken.None
            );

            // Then
            actual.Should().Be(
                new OnServerStartupResult(
                    true
                )
            );

            fileSaverMock.Verify(
                mock => mock.SaveToFile(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<ZoneMapDetails>()
                ),
                Times.Never()
            );
        }
    }
}
