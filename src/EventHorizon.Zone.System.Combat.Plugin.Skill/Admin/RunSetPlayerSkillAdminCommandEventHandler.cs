namespace EventHorizon.Zone.System.Combat.Plugin.Skill.Admin
{
    using EventHorizon.Zone.Core.Events.Entity.Update;
    using EventHorizon.Zone.Core.Model.DateTimeService;
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.Core.Model.Player;
    using EventHorizon.Zone.System.Admin.Plugin.Command.Events;
    using EventHorizon.Zone.System.Admin.Plugin.Command.Model.Standard;
    using EventHorizon.Zone.System.Combat.Model;
    using EventHorizon.Zone.System.Combat.Plugin.Skill.Find;
    using EventHorizon.Zone.System.Combat.Plugin.Skill.Model.Entity;

    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    public class RunSetPlayerSkillAdminCommandEventHandler
        : INotificationHandler<AdminCommandEvent>
    {
        private readonly IMediator _mediator;
        private readonly IPlayerRepository _playerRepository;
        private readonly IDateTimeService _dateTime;

        public RunSetPlayerSkillAdminCommandEventHandler(
            IMediator mediator,
            IPlayerRepository playerRepository,
            IDateTimeService dateTime
        )
        {
            _mediator = mediator;
            _playerRepository = playerRepository;
            _dateTime = dateTime;
        }

        public async Task Handle(
            AdminCommandEvent notification,
            CancellationToken cancellationToken
        )
        {
            var command = notification.Command;
            if (command.Command != "set-player-skill")
            {
                return;
            }
            if (command.Parts.Count != 2)
            {
                await _mediator.Send(
                    new RespondToAdminCommand(
                        notification.ConnectionId,
                        new StandardAdminCommandResponse(
                            command.Command,
                            command.RawCommand,
                            false,
                            "not_valid_command"
                        )
                    )
                );
                return;
            }
            var playerId = command.Parts[0];
            var skillId = command.Parts[1];
            var player = await _playerRepository.FindById(
                    playerId
            );
            if (!player.IsFound())
            {
                await _mediator.Send(
                    new RespondToAdminCommand(
                        notification.ConnectionId,
                        new StandardAdminCommandResponse(
                            command.Command,
                            command.RawCommand,
                            false,
                            "player_not_found"
                        )
                    )
                );
                return;
            }
            var skill = await _mediator.Send(
                new FindSkillByIdEvent
                {
                    SkillId = skillId
                }
            );
            if (!skill.IsFound())
            {
                await _mediator.Send(
                    new RespondToAdminCommand(
                        notification.ConnectionId,
                        new StandardAdminCommandResponse(
                            command.Command,
                            command.RawCommand,
                            false,
                            "skill_not_found"
                        )
                    )
                );
                return;
            }
            var playerSkillState = player.GetProperty<SkillState>(
                SkillState.PROPERTY_NAME
            );
            if (!playerSkillState.SkillMap.Contains(
                skill.Id
            ))
            {
                // Add the skill
                playerSkillState.SkillMap = playerSkillState.SkillMap.Set(
                    new SkillStateDetails
                    {
                        Id = skill.Id,
                        CooldownFinishes = _dateTime.Now,
                    }
                );
                player.SetProperty(
                    SkillState.PROPERTY_NAME,
                    playerSkillState
                );
                await _mediator.Send(
                    new UpdateEntityCommand(
                        EntitySkillAction.ADD_SKILL,
                        player
                    )
                );
            }
            await _mediator.Send(
                new RespondToAdminCommand(
                    notification.ConnectionId,
                    new StandardAdminCommandResponse(
                        command.Command,
                        command.RawCommand,
                        true,
                        "skill_added_to_player"
                    )
                )
            );
        }
    }
}
