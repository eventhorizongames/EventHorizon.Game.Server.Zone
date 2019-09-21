using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Model.Client.DataType;
using EventHorizon.Zone.Core.Events.Client;
using EventHorizon.Zone.Core.Events.Client.Actions;
using EventHorizon.Zone.Core.Model.Client;
using EventHorizon.Game.Server.Zone.Player;
using EventHorizon.Game.Server.Zone.Player.State;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace EventHorizon.Game.Server.Zone.Client.Handler
{

    public class ClientActionEntityClientMoveHandler : ClientActionToAllHandler<ClientActionEntityClientMoveToAllEvent, EntityClientMoveData>, INotificationHandler<ClientActionEntityClientMoveToAllEvent>
    {
        public ClientActionEntityClientMoveHandler(
                IMediator mediator
            ) : base(mediator)
        {
        }
    }
}