namespace EventHorizon.Zone.System.ClientEntities.Tests.Lifetime
{
    using EventHorizon.Zone.Core.Events.DirectoryService;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.Core.Model.Lifetime;
    using EventHorizon.Zone.System.ClientEntities.Lifetime;

    using FluentAssertions;

    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    using Microsoft.Extensions.Logging;

    using Moq;

    using Xunit;

    public class OnStartupSetupClientEntitiesSystemCommandHandlerTests
    {
        [Fact]
        public async Task ShouldCreateClientEntityDirectoryWhenDoesNotExist()
        {
            // Given
            var clientEntityPath = "client-entity-path";
            var expected = new CreateDirectory(
                clientEntityPath
            );

            var loggerMock = new Mock<ILogger<OnStartupSetupClientEntitiesSystemCommandHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();

            serverInfoMock.Setup(
                mock => mock.ClientEntityPath
            ).Returns(
                clientEntityPath
            );

            // When
            var handler = new OnStartupSetupClientEntitiesSystemCommandHandler(
                loggerMock.Object,
                mediatorMock.Object,
                serverInfoMock.Object
            );
            var actual = await handler.Handle(
                new OnStartupSetupClientEntitiesSystemCommand(),
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
        public async Task ShouldNotCreateClientEntityDirectoryWhenAlreadyExisting()
        {
            // Given
            var clientEntityPath = "client-entity-path";

            var loggerMock = new Mock<ILogger<OnStartupSetupClientEntitiesSystemCommandHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();

            serverInfoMock.Setup(
                mock => mock.ClientEntityPath
            ).Returns(
                clientEntityPath
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new DoesDirectoryExist(
                        clientEntityPath
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                true
            );

            // When
            var handler = new OnStartupSetupClientEntitiesSystemCommandHandler(
                loggerMock.Object,
                mediatorMock.Object,
                serverInfoMock.Object
            );
            var actual = await handler.Handle(
                new OnStartupSetupClientEntitiesSystemCommand(),
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
