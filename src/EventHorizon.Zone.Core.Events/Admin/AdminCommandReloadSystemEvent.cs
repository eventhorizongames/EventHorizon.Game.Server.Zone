using MediatR;

namespace EventHorizon.Zone.Core.Events.Admin
{
    // TODO: Delete this after refactor
    public struct AdminCommandReloadSystemEvents : INotification
    {
        public object Data { get; set; }
    }
}