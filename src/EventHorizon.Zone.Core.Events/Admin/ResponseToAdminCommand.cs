using EventHorizon.Zone.Core.Model.Admin;
using MediatR;

namespace EventHorizon.Zone.Core.Events.Admin.Command.Respond
{
    public struct ResponseToAdminCommand : IRequest<bool>
    {

        public string ConnectionId { get; }
        public IAdminCommandResponse Response { get; }

        public ResponseToAdminCommand(
            string connectionId,
            IAdminCommandResponse response
        )
        {
            this.ConnectionId = connectionId;
            this.Response = response;
        }
    }
}