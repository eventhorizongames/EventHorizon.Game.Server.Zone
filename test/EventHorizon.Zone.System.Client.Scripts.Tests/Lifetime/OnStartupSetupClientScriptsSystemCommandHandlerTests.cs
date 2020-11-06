namespace EventHorizon.Zone.System.Client.Scripts.Tests.Lifetime
{
    using System;
    using EventHorizon.Zone.Core.Events.DirectoryService;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.Core.Model.Lifetime;
    using EventHorizon.Zone.System.Client.Scripts.Lifetime;
    using FluentAssertions;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    public class OnStartupSetupClientScriptsSystemCommandHandlerTests
    {
        [Fact]
        public async Task ShouldCreateClientWhenDoesNotExist()
        {
            // Given
            var clientScriptsPath = "client-script-path";
            var expected = new CreateDirectory(
                clientScriptsPath
            );

            var loggerMock = new Mock<ILogger<OnStartupSetupClientScriptsSystemCommandHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();

            serverInfoMock.Setup(
                mock => mock.ClientScriptsPath
            ).Returns(
                clientScriptsPath
            );

            // When
            var handler = new OnStartupSetupClientScriptsSystemCommandHandler(
                loggerMock.Object,
                mediatorMock.Object,
                serverInfoMock.Object
            );
            var actual = await handler.Handle(
                new OnStartupSetupClientScriptsSystemCommand(),
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
        public async Task ShouldNotCreateDirectoryWhenAlreadyExisting()
        {
            // Given
            var clientScriptsPath = "client-script-path";

            var loggerMock = new Mock<ILogger<OnStartupSetupClientScriptsSystemCommandHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();

            serverInfoMock.Setup(
                mock => mock.ClientScriptsPath
            ).Returns(
                clientScriptsPath
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new DoesDirectoryExist(
                        clientScriptsPath
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                true
            );

            // When
            var handler = new OnStartupSetupClientScriptsSystemCommandHandler(
                loggerMock.Object,
                mediatorMock.Object,
                serverInfoMock.Object
            );
            var actual = await handler.Handle(
                new OnStartupSetupClientScriptsSystemCommand(),
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
