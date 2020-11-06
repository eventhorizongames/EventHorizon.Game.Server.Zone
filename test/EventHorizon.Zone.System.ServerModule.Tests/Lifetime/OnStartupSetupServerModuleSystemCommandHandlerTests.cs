namespace EventHorizon.Zone.System.ServerModule.Tests.Lifetime
{
    using System;
    using EventHorizon.Zone.Core.Events.DirectoryService;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.Core.Model.Lifetime;
    using EventHorizon.Zone.System.ServerModule.Lifetime;
    using FluentAssertions;
    using global::System.IO;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    public class OnStartupSetupServerModuleSystemCommandHandlerTests
    {
        [Fact]
        public async Task ShouldCreateClientParticlesDirectoryWhenDoesNotExist()
        {
            // Given
            var clientPath = "client-path";
            var serverModulePath = Path.Combine(
                clientPath,
                "ServerModule"
            );
            var expected = new CreateDirectory(
                serverModulePath
            );

            var loggerMock = new Mock<ILogger<OnStartupSetupServerModuleSystemCommandHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();

            serverInfoMock.Setup(
                mock => mock.ClientPath
            ).Returns(
                clientPath
            );

            // When
            var handler = new OnStartupSetupServerModuleSystemCommandHandler(
                loggerMock.Object,
                mediatorMock.Object,
                serverInfoMock.Object
            );
            var actual = await handler.Handle(
                new OnStartupSetupServerModuleSystemCommand(),
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
            var clientPath = "client-path";
            var serverModulePath = Path.Combine(
                clientPath,
                "ServerModule"
            );

            var loggerMock = new Mock<ILogger<OnStartupSetupServerModuleSystemCommandHandler>>();
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
                        serverModulePath
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                true
            );

            // When
            var handler = new OnStartupSetupServerModuleSystemCommandHandler(
                loggerMock.Object,
                mediatorMock.Object,
                serverInfoMock.Object
            );
            var actual = await handler.Handle(
                new OnStartupSetupServerModuleSystemCommand(),
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
