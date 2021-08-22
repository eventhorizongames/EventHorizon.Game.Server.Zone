namespace EventHorizon.Zone.System.Player.Events.Details
{
    using EventHorizon.Zone.System.Player.Model.Details;

    using MediatR;

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
