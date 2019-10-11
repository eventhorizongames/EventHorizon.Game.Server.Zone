using System.Linq;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Player.Plugin.Action;
using Microsoft.AspNetCore.SignalR;

namespace EventHorizon.Zone.System.Player.ExternalHub
{
    public partial class PlayerHub : Hub
    {
        public async Task PlayerAction(
            string actionName,
            dynamic actionData
        )
        {
            await _mediator.Publish(
                new PlayerActionEvent(
                    Context.User.Claims.FirstOrDefault(
                        claim => claim.Type == "sub"
                    )?.Value,
                    actionName,
                    actionData
                )
            );
        }
    }
}