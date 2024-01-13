namespace EventHorizon.Zone.System.Player.Tests.Lifetime;

using AutoFixture.Xunit2;

using EventHorizon.Test.Common.Attributes;
using EventHorizon.Zone.Core.Events.DirectoryService;
using EventHorizon.Zone.Core.Events.FileService;
using EventHorizon.Zone.Core.Model.Command;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.Core.Model.Lifetime;
using EventHorizon.Zone.System.Player.Lifetime;
using EventHorizon.Zone.System.Player.Model;

using FluentAssertions;

using global::System.IO;
using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

using Moq;

using Xunit;

public class OnStartupSetupPlayerSystemCommandHandlerTests
{
    [Theory, AutoMoqData]
    public async Task ShouldCreatePlayerDirectoryWhenDoesNotExist(
        // Given
        string appDataPath,
        [Frozen] Mock<IMediator> mediatorMock,
        [Frozen] Mock<ServerInfo> serverInfoMock,
        OnStartupSetupPlayerSystemCommandHandler handler
    )
    {
        var expected = new CreateDirectory(
            Path.Combine(
                appDataPath,
                PlayerSystemConstants.PlayerAppDataPath
            )
        );

        serverInfoMock.Setup(
            mock => mock.AppDataPath
        ).Returns(
            appDataPath
        );

        // When
        var actual = await handler.Handle(
            new OnStartupSetupPlayerSystemCommand(),
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

    [Theory, AutoMoqData]
    public async Task ShouldIgnoreFailureResultsWhenWriteResourceToFileHasFailureErrorCode(
        // Given
        string errorCode,
        string appDataPath,
        [Frozen] Mock<IMediator> mediatorMock,
        [Frozen] Mock<ServerInfo> serverInfoMock,
        OnStartupSetupPlayerSystemCommandHandler handler
    )
    {
        serverInfoMock.Setup(
            mock => mock.AppDataPath
        ).Returns(
            appDataPath
        );

        mediatorMock.Setup(
            mock => mock.Send(
                It.IsAny<WriteResourceToFile>(),
                CancellationToken.None
            )
        ).ReturnsAsync(
            new StandardCommandResult(
                errorCode
            )
        );

        // When
        var actual = await handler.Handle(
            new OnStartupSetupPlayerSystemCommand(),
            CancellationToken.None
        );

        // Then
        actual.Should().Be(
            new OnServerStartupResult(
                true
            )
        );
    }

    [Theory, AutoMoqData]
    public async Task ShouldNotCreatePlayerDirectoryWhenAlreadyExisting(
        // Given
        string appDataPath,
        [Frozen] Mock<IMediator> mediatorMock,
        [Frozen] Mock<ServerInfo> serverInfoMock,
        OnStartupSetupPlayerSystemCommandHandler handler
    )
    {
        var playerPath = Path.Combine(
            appDataPath,
            PlayerSystemConstants.PlayerAppDataPath
        );

        serverInfoMock.Setup(
            mock => mock.AppDataPath
        ).Returns(
            appDataPath
        );

        mediatorMock.Setup(
            mock => mock.Send(
                new DoesDirectoryExist(
                    playerPath
                ),
                CancellationToken.None
            )
        ).ReturnsAsync(
            true
        );

        // When
        var actual = await handler.Handle(
            new OnStartupSetupPlayerSystemCommand(),
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
