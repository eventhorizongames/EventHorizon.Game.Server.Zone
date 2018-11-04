using EventHorizon.Game.Server.Zone.Admin.Command.Model;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Admin.Command.ReloadCombatSystem
{
    public struct AdminCommandReloadCombatSystemEvent : IRequest<AdminCommandResponse>
    {
        public object Data { get; set; }
    }
}