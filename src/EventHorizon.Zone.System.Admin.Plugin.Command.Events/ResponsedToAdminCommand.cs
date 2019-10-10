using EventHorizon.Zone.System.Admin.Plugin.Command.Model;
using MediatR;

namespace EventHorizon.Zone.System.Admin.Plugin.Command.Events
{
    public struct RespondToAdminCommand : IRequest<bool>
    {
        public string ConnectionId { get; }
        public IAdminCommandResponse Response { get; }

        public RespondToAdminCommand(
            string connectionId,
            IAdminCommandResponse response
        )
        {
            this.ConnectionId = connectionId;
            this.Response = response;
        }
    }
}