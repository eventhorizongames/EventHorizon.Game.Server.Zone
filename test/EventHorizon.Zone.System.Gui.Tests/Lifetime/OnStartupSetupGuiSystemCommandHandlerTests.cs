namespace EventHorizon.Zone.System.ClientAssets.Tests.Lifetime
{
    using EventHorizon.Zone.Core.Events.DirectoryService;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.Core.Model.Lifetime;
    using EventHorizon.Zone.System.Gui.Lifetime;
    using FluentAssertions;
    using global::System.IO;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    public class OnStartupSetupGuiSystemCommandHandlerTests
    {
        [Fact]
        public async Task ShouldCreateGuiDirectoryWhenDoesNotExist()
        {
            // Given
            var clientPath = "client-path";
            var expected = new CreateDirectory(
                Path.Combine(
                    clientPath,
                    "Gui"
                )
            );

            var loggerMock = new Mock<ILogger<OnStartupSetupGuiSystemCommandHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();

            serverInfoMock.Setup(
                mock => mock.ClientPath
            ).Returns(
                clientPath
            );

            // When
            var handler = new OnStartupSetupGuiSystemCommandHandler(
                loggerMock.Object,
                mediatorMock.Object,
                serverInfoMock.Object
            );
            var actual = await handler.Handle(
                new OnStartupSetupGuiSystemCommand(),
                CancellationToken.None
            );

            // Then
            actual.Should().Be(
                new OnServerStartupResult(
                    true
                )
            );

            mediatorMock.Verify(
                mock => mock.Send(
                    expected,
                    CancellationToken.None
                )
            );
        }

        [Fact]
        public async Task ShouldNotCreateGuiDirectoryWhenAlreadyExisting()
        {
            // Given
            var clientPath = "client-path";
            var baseModulesPath = Path.Combine(
                clientPath,
                "Gui"
            );

            var loggerMock = new Mock<ILogger<OnStartupSetupGuiSystemCommandHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();

            serverInfoMock.Setup(
                mock => mock.ClientPath
            ).Returns(
                clientPath
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new DoesDirectoryExist(
                        baseModulesPath
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                true
            );

            // When
            var handler = new OnStartupSetupGuiSystemCommandHandler(
                loggerMock.Object,
                mediatorMock.Object,
                serverInfoMock.Object
            );
            var actual = await handler.Handle(
                new OnStartupSetupGuiSystemCommand(),
                CancellationToken.None
            );

            // Then
            actual.Should().Be(
                new OnServerStartupResult(
                    true
                )
            );

            mediatorMock.Verify(
                mock => mock.Send(
                    It.IsAny<CreateDirectory>(),
                    CancellationToken.None
                ),
                Times.Never()
            );
        }
    }
}
