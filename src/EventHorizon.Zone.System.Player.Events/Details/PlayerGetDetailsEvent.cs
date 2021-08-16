using EventHorizon.Zone.System.Player.Model.Details;

using MediatR;

namespace EventHorizon.Zone.System.Player.Events.Details
{
    public class PlayerGetDetailsEvent : IRequest<PlayerDetails>
    {
        public string Id { get; }

        public PlayerGetDetailsEvent(
            string id
        )
        {
            Id = id;
        }
    }
}
