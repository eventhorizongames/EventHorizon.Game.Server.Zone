namespace EventHorizon.Zone.System.Admin.Plugin.Command.Tests.Lifetime;

using EventHorizon.Zone.Core.Events.DirectoryService;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.Core.Model.Lifetime;
using EventHorizon.Zone.System.Admin.Plugin.Command.Lifetime;

using FluentAssertions;

using global::System.IO;
using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

using Microsoft.Extensions.Logging;

using Moq;

using Xunit;

public class OnStartupSetupAdminCommandsPluginCommandHandlerTests
{
    [Fact]
    public async Task ShouldCreateAdminCommandsDirectoryWhenDoesNotExist()
    {
        // Given
        var adminPath = "admin-path";
        var expected = new CreateDirectory(
            Path.Combine(
                adminPath,
                "Commands"
            )
        );

        var loggerMock = new Mock<ILogger<OnStartupSetupAdminCommandsPluginCommandHandler>>();
        var mediatorMock = new Mock<IMediator>();
        var serverInfoMock = new Mock<ServerInfo>();

        serverInfoMock.Setup(
            mock => mock.AdminPath
        ).Returns(
            adminPath
        );

        // When
        var handler = new OnStartupSetupAdminCommandsPluginCommandHandler(
            loggerMock.Object,
            mediatorMock.Object,
            serverInfoMock.Object
        );
        var actual = await handler.Handle(
            new OnStartupSetupAdminCommandsPluginCommand(),
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
    public async Task ShouldNotCreateAdminCommandsDirectoryWhenAlreadyExisting()
    {
        // Given
        var adminPath = "admin-path";
        var commandsPath = Path.Combine(
            adminPath,
            "Commands"
        );

        var loggerMock = new Mock<ILogger<OnStartupSetupAdminCommandsPluginCommandHandler>>();
        var mediatorMock = new Mock<IMediator>();
        var serverInfoMock = new Mock<ServerInfo>();

        serverInfoMock.Setup(
            mock => mock.AdminPath
        ).Returns(
            adminPath
        );

        mediatorMock.Setup(
            mock => mock.Send(
                new DoesDirectoryExist(
                    commandsPath
                ),
                CancellationToken.None
            )
        ).ReturnsAsync(
            true
        );

        // When
        var handler = new OnStartupSetupAdminCommandsPluginCommandHandler(
            loggerMock.Object,
            mediatorMock.Object,
            serverInfoMock.Object
        );
        var actual = await handler.Handle(
            new OnStartupSetupAdminCommandsPluginCommand(),
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
