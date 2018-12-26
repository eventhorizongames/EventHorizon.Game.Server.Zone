using MediatR;

namespace EventHorizon.Game.Server.Zone.Events.Admin
{
    public struct AdminCommandReloadSystemEvent : INotification
    {
        public object Data { get; set; }
    }
}