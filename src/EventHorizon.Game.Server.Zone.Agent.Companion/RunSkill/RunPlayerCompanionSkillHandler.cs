using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Agent.Companion.Model;
using EventHorizon.Game.Server.Zone.External.Entity;
using EventHorizon.Game.Server.Zone.Model.Entity;
using EventHorizon.Plugin.Zone.System.Combat.Events.Skill.Runner;
using MediatR;
using EventHorizon.Plugin.Zone.System.Combat.Client.Messsage;
using EventHorizon.Plugin.Zone.System.Combat.Events.Client.Messsage;
using EventHorizon.Game.I18n;
using EventHorizon.Game.I18n.Model;

namespace EventHorizon.Game.Server.Zone.Agent.Companion.RunSkill
{
    public struct RunPlayerCompanionSkillHandler : INotificationHandler<RunPlayerCompanionSkillEvent>
    {
        readonly IMediator _mediator;
        readonly IEntityRepository _entityRepository;
        readonly I18nResolver _i18nResolver;

        public RunPlayerCompanionSkillHandler(
            IMediator mediator,
            IEntityRepository entityRepository,
            I18nResolver i18nResolver
        )
        {
            _mediator = mediator;
            _entityRepository = entityRepository;
            _i18nResolver = i18nResolver;
        }

        public async Task Handle(RunPlayerCompanionSkillEvent notification, CancellationToken cancellationToken)
        {
            // Get the Caster Entity
            var casterEntity = await _entityRepository.FindById(
                notification.CasterId
            );
            // Get the Player Entity
            var playerEntity = await _entityRepository.FindById(
                notification.PlayerId
            );
            // Validate that the Owner of caster is player
            if (casterEntity
                .GetProperty<OwnerState>(
                    OwnerState.PROPERTY_NAME
                ).OwnerId != playerEntity.GlobalId)
            {
                // Send Message to Connection, Code: skill_exception
                await _mediator.Publish(
                    new SingleClientActionMessageFromCombatSystemEvent
                    {
                        ConnectionId = notification.ConnectionId,
                        Data = new MessageFromCombatSystemData
                        {
                            MessageCode = "player_not_companion_owner",
                            Message = _i18nResolver.Resolve(
                                "default",
                                "playerNotCompanionOwner",
                                new I18nTokenValue
                                {
                                    Token = "playerName",
                                    Value = playerEntity.Name
                                },
                                new I18nTokenValue
                                {
                                    Token = "casterName",
                                    Value = casterEntity.Name
                                }
                            )
                        }
                    }
                ).ConfigureAwait(false);
            }

            await _mediator.Publish(
                new RunSkillWithTargetOfEntityEvent
                {
                    ConnectionId = notification.ConnectionId,
                    CasterId = notification.CasterId,
                    SkillId = notification.SkillId,
                    TargetId = notification.TargetId,
                    TargetPosition = notification.TargetPosition
                }
            );
        }
    }
}