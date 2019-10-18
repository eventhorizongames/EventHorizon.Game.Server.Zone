using System.Collections.Generic;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Player.Plugin.Action.Events;
using Microsoft.AspNetCore.SignalR;

namespace EventHorizon.Zone.System.Player.ExternalHub
{
    public partial class PlayerHub : Hub
    {
        public async Task PlayerAction(
            string actionName,
            IDictionary<string, object> actionData
        )
        {
            await _mediator.Publish(
                new RunPlayerServerAction(
                    GetPlayerId(),
                    actionName,
                    actionData
                )
            );
        }
    }
}