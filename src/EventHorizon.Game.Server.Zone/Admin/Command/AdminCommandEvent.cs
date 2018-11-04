using EventHorizon.Game.Server.Zone.Admin.Command.Model;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Admin.Command
{
    public class AdminCommandEvent : IRequest<AdminCommandResponse>
    {
        public string Command { get; set; }
        public object Data { get; set; }
    }
}