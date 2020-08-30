namespace EventHorizon.Zone.System.Player.ExternalHub
{
    using global::System.Collections.Generic;
    using global::System.Threading.Tasks;
    using EventHorizon.Zone.System.Player.Plugin.Action.Events;
    using Microsoft.AspNetCore.SignalR;

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