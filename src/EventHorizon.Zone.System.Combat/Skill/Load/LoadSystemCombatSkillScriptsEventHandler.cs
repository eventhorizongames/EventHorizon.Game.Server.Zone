using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace EventHorizon.Zone.System.Combat.Skill.Load
{
    public class LoadSystemCombatSkillScriptsEventHandler : INotificationHandler<LoadSystemCombatSkillScriptsEvent>
    {
        readonly IMediator _mediator;

        public LoadSystemCombatSkillScriptsEventHandler(
            IMediator mediator
        )
        {
            _mediator = mediator;
        }
        public async Task Handle(
            LoadSystemCombatSkillScriptsEvent notification,
            CancellationToken cancellationToken
        )
        {
            await _mediator.Send(
                new LoadCombatSkillEffectScripts()
            );
            await _mediator.Send(
                new LoadCombatSkillValidatorScripts()
            );
        }
    }
}