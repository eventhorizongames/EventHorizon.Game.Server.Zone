namespace EventHorizon.Zone.System.Combat.Plugin.Skill.Tests.Lifetime
{
    using EventHorizon.Zone.Core.Events.DirectoryService;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.Core.Model.Lifetime;
    using EventHorizon.Zone.System.Combat.Plugin.Skill.Lifetime;

    using FluentAssertions;

    using global::System.Collections.Generic;
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
        public async Task ShouldCreateSkillDirectoriesWhenDoNotExist()
        {
            // Given
            var clientPath = "client-path";
            var serverScriptsPath = "server-scripts-path";
            var expectedList = new List<CreateDirectory>
            {
                new CreateDirectory(
                    Path.Combine(
                        clientPath,
                        "Skills"
                    )
                ),
                new CreateDirectory(
                    Path.Combine(
                        serverScriptsPath,
                        "Effects"
                    )
                ),
                new CreateDirectory(
                    Path.Combine(
                        serverScriptsPath,
                        "Validators"
                    )
                ),
            };

            var loggerMock = new Mock<ILogger<OnStartupSetupCombatSkillPluginCommandHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();

            serverInfoMock.Setup(
                mock => mock.ClientPath
            ).Returns(
                clientPath
            );
            serverInfoMock.Setup(
                mock => mock.ServerScriptsPath
            ).Returns(
                serverScriptsPath
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
        public async Task ShouldNotCreateSkillDirectoriesWhenAlreadyExisting()
        {
            // Given
            var clientPath = "client-path";
            var serverScriptsPath = "server-scripts-path";
            var skillsPath = Path.Combine(
                clientPath,
                "Skills"
            );
            var effectsPath = Path.Combine(
                serverScriptsPath,
                "Effects"
            );
            var validatorsPath = Path.Combine(
                serverScriptsPath,
                "Validators"
            );

            var loggerMock = new Mock<ILogger<OnStartupSetupCombatSkillPluginCommandHandler>>();
            var mediatorMock = new Mock<IMediator>();
            var serverInfoMock = new Mock<ServerInfo>();

            serverInfoMock.Setup(
                mock => mock.ClientPath
            ).Returns(
                clientPath
            );
            serverInfoMock.Setup(
                mock => mock.ServerScriptsPath
            ).Returns(
                serverScriptsPath
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
            mediatorMock.Setup(
                mock => mock.Send(
                    new DoesDirectoryExist(
                        effectsPath
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                true
            );
            mediatorMock.Setup(
                mock => mock.Send(
                    new DoesDirectoryExist(
                        validatorsPath
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
