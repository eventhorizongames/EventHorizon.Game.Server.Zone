using EventHorizon.Zone.Core.Model.Admin;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Admin.Command
{
    public struct AdminCommandEvent : INotification
    {
        public string ConnectionId { get; }
        public IAdminCommand Command { get; }
        public object Data { get; }

        public AdminCommandEvent(
            string connectionId,
            IAdminCommand command,
            object data
        )
        {
            this.ConnectionId = connectionId;
            this.Command = command;
            this.Data = data;
        }
        public AdminCommandEvent(
            IAdminCommand command,
            object data
        )
        {
            this.ConnectionId = null;
            this.Command = command;
            this.Data = data;
        }

    }
}