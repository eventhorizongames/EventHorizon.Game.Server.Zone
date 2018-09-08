using System;
using MediatR;

namespace EventHorizon.Game.Server.Zone.ServerAction.Model
{
    public struct ServerActionEntity
    {
        public Guid _guid;
        public DateTime RunAt { get; private set; }
        public INotification EventToSend { get; private set; }

        public ServerActionEntity(DateTime runAt, INotification eventToSend)
        {
            _guid = Guid.NewGuid();
            this.RunAt = runAt;
            this.EventToSend = eventToSend;
        }
    }
}