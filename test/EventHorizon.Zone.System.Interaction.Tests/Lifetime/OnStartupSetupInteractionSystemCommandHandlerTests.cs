namespace EventHorizon.Zone.System.Interaction.Tests.Lifetime;

using EventHorizon.Zone.Core.Events.DirectoryService;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.Core.Model.Lifetime;
using EventHorizon.Zone.System.Interaction.Lifetime;

using FluentAssertions;

using global::System.IO;
using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

using Microsoft.Extensions.Logging;

using Moq;

using Xunit;

public class OnStartupSetupInteractionSystemCommandHandlerTests
{
    [Fact]
    public async Task ShouldCreateInteractionDirectoryWhenDoesNotExist()
    {
        // Given
        var serverScriptsPath = "server-scripts-path";
        var expected = new CreateDirectory(
            Path.Combine(
                serverScriptsPath,
                "Interaction"
            )
        );

        var loggerMock = new Mock<ILogger<OnStartupSetupInteractionSystemCommandHandler>>();
        var mediatorMock = new Mock<IMediator>();
        var serverInfoMock = new Mock<ServerInfo>();

        serverInfoMock.Setup(
            mock => mock.ServerScriptsPath
        ).Returns(
            serverScriptsPath
        );

        // When
        var handler = new OnStartupSetupInteractionSystemCommandHandler(
            loggerMock.Object,
            mediatorMock.Object,
            serverInfoMock.Object
        );
        var actual = await handler.Handle(
            new OnStartupSetupInteractionSystemCommand(),
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
    public async Task ShouldNotCreateInteractionDirectoryWhenAlreadyExisting()
    {
        // Given
        var serverScriptsPath = "server-scripts-path";
        var interactionPath = Path.Combine(
            serverScriptsPath,
            "Interaction"
        );

        var loggerMock = new Mock<ILogger<OnStartupSetupInteractionSystemCommandHandler>>();
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
                    interactionPath
                ),
                CancellationToken.None
            )
        ).ReturnsAsync(
            true
        );

        // When
        var handler = new OnStartupSetupInteractionSystemCommandHandler(
            loggerMock.Object,
            mediatorMock.Object,
            serverInfoMock.Object
        );
        var actual = await handler.Handle(
            new OnStartupSetupInteractionSystemCommand(),
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
