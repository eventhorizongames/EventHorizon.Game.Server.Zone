using System;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Admin.Command.Model;
using EventHorizon.Plugin.Zone.System.Combat.Skill.Load;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Admin.Command.ReloadCombatSystem
{
    public class AdminCommandReloadCombatSystemHandler : IRequestHandler<AdminCommandReloadCombatSystemEvent, AdminCommandResponse>
    {
        readonly IMediator _mediator;
        public AdminCommandReloadCombatSystemHandler(
            IMediator mediator
        )
        {
            _mediator = mediator;
        }
        public async Task<AdminCommandResponse> Handle(AdminCommandReloadCombatSystemEvent request, CancellationToken cancellationToken)
        {
            try
            {
                await _mediator.Publish(
                    new LoadCombatSkillsEvent()
                );
                await _mediator.Publish(
                    new LoadSkillCombatSystemEvent()
                );
                return new AdminCommandResponse
                {
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new AdminCommandResponse
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
    }
}