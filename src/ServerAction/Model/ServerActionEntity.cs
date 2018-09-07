using System;

namespace EventHorizon.Game.Server.Zone.ServerAction.Model
{
    public struct ServerActionEntity
    {
        public Guid _guid;
        public DateTime RunAt { get; private set; }
        public ServerActionEntity(DateTime runAt)
        {
            _guid = Guid.NewGuid();
            this.RunAt = runAt;
        }
    }
}