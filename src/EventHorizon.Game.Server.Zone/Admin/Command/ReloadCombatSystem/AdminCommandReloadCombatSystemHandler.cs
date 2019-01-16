using System;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Admin.Command.Model;
using EventHorizon.Game.Server.Zone.Events.Admin;
using EventHorizon.Plugin.Zone.System.Combat.Particle.Event;
using EventHorizon.Plugin.Zone.System.Combat.Skill.Load;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Admin.Command.ReloadCombatSystem
{
    public class AdminCommandReloadCombatSystemHandler : INotificationHandler<AdminCommandReloadSystemEvent>
    {
        readonly IMediator _mediator;
        public AdminCommandReloadCombatSystemHandler(
            IMediator mediator
        )
        {
            _mediator = mediator;
        }
        public async Task Handle(AdminCommandReloadSystemEvent request, CancellationToken cancellationToken)
        {
            await _mediator.Publish(
                new LoadCombatSkillsEvent()
            );
            await _mediator.Publish(
                new LoadSkillCombatSystemEvent()
            );
            await _mediator.Publish(
                new SetupCombatParticleSystemEvent()
            );
        }
    }
}