namespace EventHorizon.Zone.System.Admin.Tests.Lifetime
{
    using System;

    using EventHorizon.Zone.Core.Events.DirectoryService;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.Core.Model.Lifetime;
    using EventHorizon.Zone.System.Admin.Lifetime;

    using FluentAssertions;

    using global::System.IO;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    using Microsoft.Extensions.Logging;

    using Moq;

    using Xunit;

    public class OnStartupSetupAdminSystemCommandHandlerTests
    {
        [Fact]
        public async Task ShouldCreateAdminScriptsDirectoryWhenDoesNotExist()
        {
            // Given
            var serverScriptsPath = "server-scripts-path";
            var expected = new CreateDirectory(
                Path.Combine(
                    serverScriptsPath,
                    "Admin"
                )
            );

            var loggerMock = new Mock<ILogger<OnStartupSetupAdminSystemCommandHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();

            serverInfoMock.Setup(
                mock => mock.ServerScriptsPath
            ).Returns(
                serverScriptsPath
            );

            // When
            var handler = new OnStartupSetupAdminSystemCommandHandler(
                loggerMock.Object,
                mediatorMock.Object,
                serverInfoMock.Object
            );
            var actual = await handler.Handle(
                new OnStartupSetupAdminSystemCommand(),
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
        public async Task ShouldNotCreateAdminScriptsDirectoryWhenAlreadyExisting()
        {
            // Given
            var serverScriptsPath = "server-scripts-path";
            var adminScriptsPath = Path.Combine(
                serverScriptsPath,
                "Admin"
            );

            var loggerMock = new Mock<ILogger<OnStartupSetupAdminSystemCommandHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();

            serverInfoMock.Setup(
                mock => mock.ServerScriptsPath
            ).Returns(
                serverScriptsPath
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new DoesDirectoryExist(
                        adminScriptsPath
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                true
            );

            // When
            var handler = new OnStartupSetupAdminSystemCommandHandler(
                loggerMock.Object,
                mediatorMock.Object,
                serverInfoMock.Object
            );
            var actual = await handler.Handle(
                new OnStartupSetupAdminSystemCommand(),
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
