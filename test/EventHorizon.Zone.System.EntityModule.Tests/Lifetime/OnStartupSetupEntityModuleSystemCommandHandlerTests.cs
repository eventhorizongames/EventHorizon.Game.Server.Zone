namespace EventHorizon.Zone.System.EntityModule.Tests.Lifetime
{
    using EventHorizon.Zone.Core.Events.DirectoryService;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.Core.Model.Lifetime;
    using EventHorizon.Zone.System.EntityModule.Lifetime;

    using FluentAssertions;

    using global::System.Collections.Generic;
    using global::System.IO;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    using Microsoft.Extensions.Logging;

    using Moq;

    using Xunit;

    public class OnStartupSetupEntityModuleSystemCommandHandlerTests
    {
        [Fact]
        public async Task ShouldCreateModuleDirectoriesWhenDoNotExist()
        {
            // Given
            var adminPath = "admin-path";
            var clientPath = "client-path";
            var expectedList = new List<CreateDirectory>
            {
                new CreateDirectory(
                    Path.Combine(
                        clientPath,
                        "Modules",
                        "Base"
                    )
                ),
                new CreateDirectory(
                    Path.Combine(
                        clientPath,
                        "Modules",
                        "Player"
                    )
                ),
            };

            var loggerMock = new Mock<ILogger<OnStartupSetupEntityModuleSystemCommandHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();

            serverInfoMock.Setup(
                mock => mock.AdminPath
            ).Returns(
                adminPath
            );
            serverInfoMock.Setup(
                mock => mock.ClientPath
            ).Returns(
                clientPath
            );

            // When
            var handler = new OnStartupSetupEntityModuleSystemCommandHandler(
                loggerMock.Object,
                mediatorMock.Object,
                serverInfoMock.Object
            );
            var actual = await handler.Handle(
                new OnStartupSetupEntityModuleSystemCommand(),
                CancellationToken.None
            );

            // Then
            actual.Should().Be(
                new OnServerStartupResult(
                    true
                )
            );

            foreach (var expected in expectedList)
            {
                mediatorMock.Verify(
                    mock => mock.Send(
                        expected,
                        CancellationToken.None
                    )
                );
            }
        }

        [Fact]
        public async Task ShouldNotCreateModuleDirectoryWhenAlreadyExisting()
        {
            // Given
            var clientPath = "client-path";
            var baseModulesPath = Path.Combine(
                clientPath,
                "Modules",
                "Base"
            );
            var playerModulesPath = Path.Combine(
                clientPath,
                "Modules",
                "Player"
            );

            var loggerMock = new Mock<ILogger<OnStartupSetupEntityModuleSystemCommandHandler>>();
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
            mediatorMock.Setup(
                mock => mock.Send(
                    new DoesDirectoryExist(
                        playerModulesPath
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                true
            );

            // When
            var handler = new OnStartupSetupEntityModuleSystemCommandHandler(
                loggerMock.Object,
                mediatorMock.Object,
                serverInfoMock.Object
            );
            var actual = await handler.Handle(
                new OnStartupSetupEntityModuleSystemCommand(),
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
