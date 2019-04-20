using EventHorizon.Zone.System.ClientEntities.Model;
using MediatR;

namespace EventHorizon.Zone.System.ClientEntities.Register
{
    public struct RegisterClientEntityInstanceEvent : INotification
    {
        public ClientEntityInstance ClientEntityInstance { get; set; }
    }
}