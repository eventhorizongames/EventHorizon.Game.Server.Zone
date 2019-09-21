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
            public Task Handle(
                LoadSystemCombatSkillScriptsEvent notification,
                CancellationToken cancellationToken
            )
            {
                _mediator.Send(
                    new LoadCombatSkillEffectScripts()
                );
                _mediator.Send(
                    new LoadCombatSkillValidatorScripts()
                );
                return Task.CompletedTask;
            }
        }
    }
}