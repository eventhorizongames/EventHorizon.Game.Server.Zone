using System;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Events.ServerAction
{
    public struct AddServerActionEvent : INotification
    {
        public DateTime RunAt { get; private set; }

        public INotification EventToSend { get; private set; }
        public AddServerActionEvent(DateTime runAt, INotification eventToSend)
        {
            this.RunAt = runAt;
            this.EventToSend = eventToSend;
        }
    }
}