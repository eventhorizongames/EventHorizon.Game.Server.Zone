using System.Collections.Generic;
using MediatR;

namespace EventHorizon.Zone.System.Player.Plugin.Action.Events
{
    public struct RunPlayerServerAction : INotification
    {
        public string PlayerId { get; }
        public string Action { get; }
        public IDictionary<string, object> Data { get; }

        public RunPlayerServerAction(
            string playerId,
            string action,
            IDictionary<string, object> data
        )
        {
            this.PlayerId = playerId;
            this.Action = action;
            this.Data = data;
        }
    }
}