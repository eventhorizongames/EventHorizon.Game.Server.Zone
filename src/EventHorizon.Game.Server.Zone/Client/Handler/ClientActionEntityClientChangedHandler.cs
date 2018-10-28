using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Client.DataType;
using EventHorizon.Game.Server.Zone.Events.Client;
using EventHorizon.Game.Server.Zone.Events.Client.Actions;
using EventHorizon.Game.Server.Zone.External.Client;
using EventHorizon.Game.Server.Zone.Player;
using EventHorizon.Game.Server.Zone.Player.State;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace EventHorizon.Game.Server.Zone.Client.Handler
{

    public class ClientActionEntityClientChangedHandler : ClientActionHandler<ClientActionEntityClientChangedEvent, EntityChangedData>, INotificationHandler<ClientActionEntityClientChangedEvent>
    {
        public ClientActionEntityClientChangedHandler(
                IMediator mediator
            ) : base(mediator)
        {
        }
    }
}