namespace EventHorizon.Zone.System.Admin.AdminClientAction.Client
{
    using MediatR;

    public struct SendToAllAdminClientsEvent
        : INotification
    {
        public string Method { get; set; }
        public object Arg1 { get; set; }
        public object Arg2 { get; set; }
    }
}
