using System;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Admin.Command.Model;
using EventHorizon.Game.Server.Zone.Agent.Ai.LoadRoutine;
using EventHorizon.Game.Server.Zone.Events.Admin;
using EventHorizon.Plugin.Zone.System.Combat.Skill.Load;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Admin.Command.ReloadAgentSystem
{
    public class AdminCommandReloadAgentSystemHandler : INotificationHandler<AdminCommandReloadSystemEvent>
    {
        readonly IMediator _mediator;
        public AdminCommandReloadAgentSystemHandler(
            IMediator mediator
        )
        {
            _mediator = mediator;
        }
        public async Task Handle(AdminCommandReloadSystemEvent request, CancellationToken cancellationToken)
        {
            await _mediator.Publish(
                new LoadAgentRoutineSystemEvent()
            );
        }
    }
}