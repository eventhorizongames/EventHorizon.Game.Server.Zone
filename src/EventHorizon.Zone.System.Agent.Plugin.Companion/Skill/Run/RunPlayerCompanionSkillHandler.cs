namespace EventHorizon.Zone.System.Agent.Plugin.Companion.Skill.Run
{
    using EventHorizon.Game.I18n;
    using EventHorizon.Game.I18n.Model;
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.Core.Model.Entity.State;
    using EventHorizon.Zone.System.Agent.Plugin.Companion.Events.Skill.Run;
    using EventHorizon.Zone.System.Agent.Plugin.Companion.Model;
    using EventHorizon.Zone.System.Combat.Events.Client.Messsage;
    using EventHorizon.Zone.System.Combat.Events.Skill.Runner;
    using EventHorizon.Zone.System.Combat.Model.Client.Messsage;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;

    public class RunPlayerCompanionSkillHandler : INotificationHandler<RunPlayerCompanionSkillEvent>
    {
        readonly IMediator _mediator;
        readonly EntityRepository _entityRepository;
        readonly I18nResolver _i18nResolver;

        public RunPlayerCompanionSkillHandler(
            IMediator mediator,
            EntityRepository entityRepository,
            I18nResolver i18nResolver
        )
        {
            _mediator = mediator;
            _entityRepository = entityRepository;
            _i18nResolver = i18nResolver;
        }

        public async Task Handle(
            RunPlayerCompanionSkillEvent notification,
            CancellationToken cancellationToken
        )
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
                // Send Message to Connection, ErrorCode: player_not_companion_owner
                await _mediator.Publish(
                    SingleClientActionMessageFromCombatSystemEvent.Create(
                        notification.ConnectionId,
                        new MessageFromCombatSystemData
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
                    )
                );
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