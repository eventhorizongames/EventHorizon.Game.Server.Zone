namespace EventHorizon.Zone.System.Combat.Plugin.Skill.Tests.Lifetime
{
    using EventHorizon.Zone.Core.Events.DirectoryService;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.Core.Model.Lifetime;
    using EventHorizon.Zone.System.Combat.Plugin.Skill.Lifetime;
    using FluentAssertions;
    using global::System.IO;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    public class OnStartupSetupCombatSkillPluginCommandHandlerTests
    {
        [Fact]
        public async Task ShouldCreateClientParticlesDirectoryWhenDoesNotExist()
        {
            // Given
            var clientPath = "client-path";
            var skillsPath = Path.Combine(
                clientPath,
                "Skills"
            );
            var expected = new CreateDirectory(
                skillsPath
            );

            var loggerMock = new Mock<ILogger<OnStartupSetupCombatSkillPluginCommandHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();

            serverInfoMock.Setup(
                mock => mock.ClientPath
            ).Returns(
                clientPath
            );

            // When
            var handler = new OnStartupSetupCombatSkillPluginCommandHandler(
                loggerMock.Object,
                mediatorMock.Object,
                serverInfoMock.Object
            );
            var actual = await handler.Handle(
                new OnStartupSetupCombatSkillPluginCommand(),
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
            var skillsPath = Path.Combine(
                clientPath,
                "Skills"
            );

            var loggerMock = new Mock<ILogger<OnStartupSetupCombatSkillPluginCommandHandler>>();
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
                        skillsPath
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                true
            );

            // When
            var handler = new OnStartupSetupCombatSkillPluginCommandHandler(
                loggerMock.Object,
                mediatorMock.Object,
                serverInfoMock.Object
            );
            var actual = await handler.Handle(
                new OnStartupSetupCombatSkillPluginCommand(),
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
