using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.System.Combat.Skill.State;
using MediatR;

namespace EventHorizon.Zone.System.Combat.Skill.Load
{
    public struct LoadSystemCombatSkillScriptsEvent : INotification
    {

        public struct LoadSystemCombatSkillScriptsHandler : INotificationHandler<LoadSystemCombatSkillScriptsEvent>
        {
            readonly IMediator _mediator;

            public LoadSystemCombatSkillScriptsHandler(
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
}