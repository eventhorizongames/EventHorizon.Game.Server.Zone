using MediatR;

namespace EventHorizon.Game.Server.Zone.Events.Admin
{
    // TODO: Delete this after refactor
    public struct AdminCommandReloadSystemEvents : INotification
    {
        public object Data { get; set; }
    }
}