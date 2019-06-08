using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Admin.Command;
using EventHorizon.Game.Server.Zone.Admin.Command.Respond;
using EventHorizon.Game.Server.Zone.Events.Admin;
using EventHorizon.Game.Server.Zone.Events.Entity.Find;
using EventHorizon.Game.Server.Zone.External.Entity;
using EventHorizon.Game.Server.Zone.External.Player;
using EventHorizon.Game.Server.Zone.Model.Admin;
using EventHorizon.Game.Server.Zone.Model.Entity;
using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Admin.Skill
{
    public struct ClearEntityOwnerAdminCommandEventHandler : INotificationHandler<AdminCommandEvent>
    {
        readonly IMediator _mediator;
        readonly IEntityRepository _entityRepository;
        public ClearEntityOwnerAdminCommandEventHandler(
            IMediator mediator,
            IEntityRepository entityRepository
        )
        {
            _mediator = mediator;
            _entityRepository = entityRepository;
        }
        // entity-clear-owner 73c73306-cc98-47ed-a73b-ce4395098ce4
        public async Task Handle(
            AdminCommandEvent notification,
            CancellationToken cancellationToken
        )
        {
            var command = notification.Command;
            if (command.Command != "entity-clear-owner")
            {
                return;
            }
            if (command.Parts.Count != 1)
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
            var globalId = command.Parts[0];
            var entityList =
                (await _entityRepository.All())
                .Where(a => a.Type != EntityType.PLAYER && a.GlobalId == globalId);
            if (entityList.Count() != 1)
            {
                await _mediator.Send(
                    new ResponseToAdminCommand(
                        notification.ConnectionId,
                        new StandardAdminCommandResponse(
                            command.Command,
                            command.RawCommand,
                            false,
                            "invalid_entity_id"
                        )
                    )
                );
                return;
            }

            // if (!player.IsFound())
            // {
            //     await _mediator.Send(
            //         new ResponseToAdminCommand(
            //             notification.ConnectionId,
            //             new StandardAdminCommandResponse(
            //                 command.Command,
            //                 command.RawCommand,
            //                 false,
            //                 "player_not_found"
            //             )
            //         )
            //     );
            //     return;
            // }
            // var skill = await _mediator.Send(
            //     new FindSkillByIdEvent
            //     {
            //         SkillId = skillId
            //     }
            // );
            // if (!skill.IsFound())
            // {
            //     await _mediator.Send(
            //         new ResponseToAdminCommand(
            //             notification.ConnectionId,
            //             new StandardAdminCommandResponse(
            //                 command.Command,
            //                 command.RawCommand,
            //                 false,
            //                 "skill_not_found"
            //             )
            //         )
            //     );
            //     return;
            // }
            // var playerSkillState = player.GetProperty<SkillState>(
            //     SkillState.PROPERTY_NAME
            // );
            // if (!playerSkillState.SkillList.ContainsKey(
            //     skill.Id
            // ))
            // {
            //     // Add the skill
            //     playerSkillState.SkillList.Add(
            //         skill.Id,
            //         new EntitySkillState
            //         {
            //             CooldownFinishes = DateTime.UtcNow
            //         }
            //     );
            //     await _playerRepository.Update(
            //         EntitySkillAction.ADD_SKILL,
            //         player
            //     );
            //     // TODO: This is not exposed, look at making Player Update an Event that will do this
            //     // await _mediator.Publish(new PlayerGlobalUpdateEvent
            //     // {
            //     //     Player = player,
            //     // });
            // }
            await _mediator.Send(
                new ResponseToAdminCommand(
                    notification.ConnectionId,
                    new StandardAdminCommandResponse(
                        command.Command,
                        command.RawCommand,
                        true,
                        "entity_owner_cleared"
                    )
                )
            );
        }
    }
}