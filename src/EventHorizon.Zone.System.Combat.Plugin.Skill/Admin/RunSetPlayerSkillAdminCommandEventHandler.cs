namespace EventHorizon.Zone.System.Combat.Plugin.Skill.Admin
{
    using global::System;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using EventHorizon.Zone.Core.Model.Player;
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.System.Combat.Plugin.Skill.Find;
    using MediatR;
    using EventHorizon.Zone.System.Combat.Model;
    using EventHorizon.Zone.System.Admin.Plugin.Command.Events;
    using EventHorizon.Zone.System.Admin.Plugin.Command.Model.Standard;
    using EventHorizon.Zone.Core.Events.Entity.Update;
    using EventHorizon.Zone.System.Combat.Plugin.Skill.Model.Entity;

    public class RunSetPlayerSkillAdminCommandEventHandler : INotificationHandler<AdminCommandEvent>
    {
        readonly IMediator _mediator;
        readonly IPlayerRepository _playerRepository;
        
        public RunSetPlayerSkillAdminCommandEventHandler(
            IMediator mediator,
            IPlayerRepository playerRepository
        )
        {
            _mediator = mediator;
            _playerRepository = playerRepository;
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
            if (playerSkillState.SkillMap.Contains(
                skill.Id
            ))
            {
                // Add the skill
                playerSkillState.SkillMap.Set(
                    new SkillStateDetails
                    {
                        Id = skill.Id,
                        CooldownFinishes = DateTime.UtcNow
                    }
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