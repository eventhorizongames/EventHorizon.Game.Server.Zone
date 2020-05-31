namespace EventHorizon.Zone.System.Combat.Plugin.Skill.Tests.Admin
{
    using EventHorizon.Zone.Core.Model.DateTimeService;
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.Core.Model.Player;
    using EventHorizon.Zone.System.Admin.Plugin.Command.Events;
    using EventHorizon.Zone.System.Admin.Plugin.Command.Model;
    using EventHorizon.Zone.System.Admin.Plugin.Command.Model.Standard;
    using EventHorizon.Zone.System.Combat.Plugin.Skill.Admin;
    using EventHorizon.Zone.System.Combat.Plugin.Skill.Find;
    using EventHorizon.Zone.System.Combat.Plugin.Skill.Model;
    using EventHorizon.Zone.System.Combat.Plugin.Skill.Model.Entity;
    using FluentAssertions;
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;
    using Moq;
    using Xunit;

    public class RunSetPlayerSkillAdminCommandEventHandlerTests
    {

        [Fact]
        public async Task ShouldSetCooldownOnSkillMapToDateTimeServiceNowWhenSkillIsNotFoundInPlayerSkillMap()
        {
            // Given
            var commandName = "set-player-skill";
            var rawCommand = "raw-command";
            var connectionId = "connection-id";
            var playerId = "player-id";
            var skillId = "skill-id";
            var commandParts = new List<string>
            {
                playerId,
                skillId,
            };
            var skillState = new SkillState
            {
                SkillMap = new SkillStateMap
                {
                    List = new List<SkillStateDetails>(),
                },
            };
            var player = new PlayerEntity
            {
                PlayerId = playerId,
            }.SetProperty(
                SkillState.PROPERTY_NAME,
                skillState
            );
            var skill = new SkillInstance
            {
                Id = skillId,
            };
            var command = new AdminCommandMock
            {
                Command = commandName,
                RawCommand = rawCommand,
                Parts = commandParts,
            };
            var now = DateTime.UtcNow;

            var mediatorMock = new Mock<IMediator>();
            var playerRepositoryMock = new Mock<IPlayerRepository>();
            var dateTimeMock = new Mock<IDateTimeService>();

            playerRepositoryMock.Setup(
                mock => mock.FindById(
                    playerId
                )
            ).ReturnsAsync(
                player
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new FindSkillByIdEvent
                    {
                        SkillId = skillId,
                    },
                    CancellationToken.None
                )
            ).ReturnsAsync(
                skill
            );

            dateTimeMock.Setup(
                mock => mock.Now
            ).Returns(
                now
            );

            // When
            var handler = new RunSetPlayerSkillAdminCommandEventHandler(
                mediatorMock.Object,
                playerRepositoryMock.Object,
                dateTimeMock.Object
            );
            await handler.Handle(
                new AdminCommandEvent(
                    connectionId,
                    command,
                    null
                ),
                CancellationToken.None
            );

            var actual = player.GetProperty<SkillState>(
                SkillState.PROPERTY_NAME
            );

            // Then
            actual.SkillMap
                .List.Should().Contain(
                    new SkillStateDetails
                    {
                        Id = skillId,
                        CooldownFinishes = now,
                    }
                );
        }

        [Fact]
        public async Task ShouldSendSkillAddedToPlayerResponseWhenSkillIsInSkillMap()
        {
            // Given
            var commandName = "set-player-skill";
            var rawCommand = "raw-command";
            var connectionId = "connection-id";
            var playerId = "player-id";
            var skillId = "skill-id";
            var commandParts = new List<string>
            {
                playerId,
                skillId,
            };
            var skillState = new SkillState
            {
                SkillMap = new SkillStateMap
                {
                    List = new List<SkillStateDetails>(),
                },
            };
            var player = new PlayerEntity
            {
                PlayerId = playerId,
            }.SetProperty(
                SkillState.PROPERTY_NAME,
                skillState
            );
            var skill = new SkillInstance
            {
                Id = skillId,
            };
            var command = new AdminCommandMock
            {
                Command = commandName,
                RawCommand = rawCommand,
                Parts = commandParts,
            };
            var now = DateTime.UtcNow;

            var mediatorMock = new Mock<IMediator>();
            var playerRepositoryMock = new Mock<IPlayerRepository>();
            var dateTimeMock = new Mock<IDateTimeService>();

            playerRepositoryMock.Setup(
                mock => mock.FindById(
                    playerId
                )
            ).ReturnsAsync(
                player
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new FindSkillByIdEvent
                    {
                        SkillId = skillId,
                    },
                    CancellationToken.None
                )
            ).ReturnsAsync(
                skill
            );

            dateTimeMock.Setup(
                mock => mock.Now
            ).Returns(
                now
            );

            // When
            var handler = new RunSetPlayerSkillAdminCommandEventHandler(
                mediatorMock.Object,
                playerRepositoryMock.Object,
                dateTimeMock.Object
            );
            await handler.Handle(
                new AdminCommandEvent(
                    connectionId,
                    command,
                    null
                ),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Send(
                    new RespondToAdminCommand(
                        connectionId,
                        new StandardAdminCommandResponse(
                            commandName,
                            rawCommand,
                            true,
                            "skill_added_to_player"
                        )
                    ),
                    CancellationToken.None
                )
            );
        }

        [Fact]
        public async Task ShouldNotDoAnythingWhenTheCommandIsNotSetPlayerSkill()
        {
            // Given
            var commandName = "not-correct-command";
            var connectionId = "connection-id";
            var command = new AdminCommandMock
            {
                Command = commandName,
            };

            var mediatorMock = new Mock<IMediator>();
            var playerRepositoryMock = new Mock<IPlayerRepository>();
            var dateTimeMock = new Mock<IDateTimeService>();

            // When
            var handler = new RunSetPlayerSkillAdminCommandEventHandler(
                mediatorMock.Object,
                playerRepositoryMock.Object,
                dateTimeMock.Object
            );
            await handler.Handle(
                new AdminCommandEvent(
                    connectionId,
                    command,
                    null
                ),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Send(
                    It.IsAny<RespondToAdminCommand>(),
                    CancellationToken.None
                ),
                Times.Never()
            );
        }

        [Fact]
        public async Task ShouldSendNotValidCommandResponseWhenPartsAreNotEqualToTwo()
        {
            // Given
            var commandName = "set-player-skill";
            var rawCommand = "raw-command";
            var connectionId = "connection-id";
            var playerId = "player-id";
            var commandParts = new List<string>
            {
                playerId,
            };
            var command = new AdminCommandMock
            {
                Command = commandName,
                RawCommand = rawCommand,
                Parts = commandParts,
            };

            var mediatorMock = new Mock<IMediator>();
            var playerRepositoryMock = new Mock<IPlayerRepository>();
            var dateTimeMock = new Mock<IDateTimeService>();

            // When
            var handler = new RunSetPlayerSkillAdminCommandEventHandler(
                mediatorMock.Object,
                playerRepositoryMock.Object,
                dateTimeMock.Object
            );
            await handler.Handle(
                new AdminCommandEvent(
                    connectionId,
                    command,
                    null
                ),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Send(
                    new RespondToAdminCommand(
                        connectionId,
                        new StandardAdminCommandResponse(
                            commandName,
                            rawCommand,
                            false,
                            "not_valid_command"
                        )
                    ),
                    CancellationToken.None
                )
            );
        }

        [Fact]
        public async Task ShouldSendPlayerNotFoundResponseWhenPlayerIsNotFound()
        {
            // Given
            var commandName = "set-player-skill";
            var rawCommand = "raw-command";
            var connectionId = "connection-id";
            var playerId = "player-id";
            var skillId = "skill-id";
            var commandParts = new List<string>
            {
                playerId,
                skillId,
            };
            var command = new AdminCommandMock
            {
                Command = commandName,
                RawCommand = rawCommand,
                Parts = commandParts,
            };

            var mediatorMock = new Mock<IMediator>();
            var playerRepositoryMock = new Mock<IPlayerRepository>();
            var dateTimeMock = new Mock<IDateTimeService>();

            playerRepositoryMock.Setup(
                mock => mock.FindById(
                    playerId
                )
            ).ReturnsAsync(
                default(PlayerEntity)
            );

            // When
            var handler = new RunSetPlayerSkillAdminCommandEventHandler(
                mediatorMock.Object,
                playerRepositoryMock.Object,
                dateTimeMock.Object
            );
            await handler.Handle(
                new AdminCommandEvent(
                    connectionId,
                    command,
                    null
                ),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Send(
                    new RespondToAdminCommand(
                        connectionId,
                        new StandardAdminCommandResponse(
                            commandName,
                            rawCommand,
                            false,
                            "player_not_found"
                        )
                    ),
                    CancellationToken.None
                )
            );
        }

        [Fact]
        public async Task ShouldReturnSkillNotFoundResponseWhenSkillIsNotFound()
        {
            // Given
            var commandName = "set-player-skill";
            var rawCommand = "raw-command";
            var connectionId = "connection-id";
            var playerId = "player-id";
            var skillId = "skill-id";
            var commandParts = new List<string>
            {
                playerId,
                skillId,
            };
            var player = new PlayerEntity
            {
                PlayerId = playerId,
            };
            var command = new AdminCommandMock
            {
                Command = commandName,
                RawCommand = rawCommand,
                Parts = commandParts,
            };

            var mediatorMock = new Mock<IMediator>();
            var playerRepositoryMock = new Mock<IPlayerRepository>();
            var dateTimeMock = new Mock<IDateTimeService>();

            playerRepositoryMock.Setup(
                mock => mock.FindById(
                    playerId
                )
            ).ReturnsAsync(
                player
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new FindSkillByIdEvent
                    {
                        SkillId = skillId,
                    },
                    CancellationToken.None
                )
            ).ReturnsAsync(
                default(SkillInstance)
            );

            // When
            var handler = new RunSetPlayerSkillAdminCommandEventHandler(
                mediatorMock.Object,
                playerRepositoryMock.Object,
                dateTimeMock.Object
            );
            await handler.Handle(
                new AdminCommandEvent(
                    connectionId,
                    command,
                    null
                ),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Send(
                    new RespondToAdminCommand(
                        connectionId,
                        new StandardAdminCommandResponse(
                            commandName,
                            rawCommand,
                            false,
                            "skill_not_found"
                        )
                    ),
                    CancellationToken.None
                )
            );
        }
    }

    public class AdminCommandMock : IAdminCommand
    {
        public string RawCommand { get; set; }
        public string Command { get; set; }
        public IList<string> Parts { get; set; }
    }
}
