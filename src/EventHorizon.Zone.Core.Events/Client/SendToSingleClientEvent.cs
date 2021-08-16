using MediatR;

namespace EventHorizon.Zone.Core.Events.Client
{
    public struct SendToSingleClientEvent : INotification
    {
        public string ConnectionId { get; set; }
        public string Method { get; set; }
        public object Arg1 { get; set; }
        public object Arg2 { get; set; }
    }
}
