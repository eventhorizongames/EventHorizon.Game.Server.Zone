using System;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Admin.Command;
using EventHorizon.Game.Server.Zone.Admin.Command.Respond;
using EventHorizon.Game.Server.Zone.Events.Admin;
using EventHorizon.Game.Server.Zone.Events.Entity.Find;
using EventHorizon.Game.Server.Zone.External.Player;
using EventHorizon.Game.Server.Zone.Model.Admin;
using EventHorizon.Game.Server.Zone.Model.Entity;
using EventHorizon.Plugin.Zone.System.Combat.Model;
using EventHorizon.Plugin.Zone.System.Combat.Skill.Entity.State;
using EventHorizon.Plugin.Zone.System.Combat.Skill.Find;
using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Admin.Skill
{
    public struct RunSetPlayerSkillAdminCommandEventHandler : INotificationHandler<AdminCommandEvent>
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
                    new ResponseToAdminCommand(
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
                    new ResponseToAdminCommand(
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
                    new ResponseToAdminCommand(
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
            if (!playerSkillState.SkillList.ContainsKey(
                skill.Id
            ))
            {
                // Add the skill
                playerSkillState.SkillList.Add(
                    skill.Id,
                    new EntitySkillState
                    {
                        CooldownFinishes = DateTime.UtcNow
                    }
                );
                await _playerRepository.Update(
                    EntitySkillAction.ADD_SKILL,
                    player
                );
                // TODO: This is not exposed, look at making Player Update an Event that will do this
                // await _mediator.Publish(new PlayerGlobalUpdateEvent
                // {
                //     Player = player,
                // });
            }
            await _mediator.Send(
                new ResponseToAdminCommand(
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