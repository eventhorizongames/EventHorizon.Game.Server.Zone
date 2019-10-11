using MediatR;
using EventHorizon.Zone.System.Player.Model.Details;

namespace EventHorizon.Zone.System.Player.Events.Details
{
    public class PlayerGetDetailsEvent : IRequest<PlayerDetails>
    {
        public string Id { get; set; }
    }
}