namespace EventHorizon.Game.Capture
{
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Zone.Core.Events.Entity.Find;
    using EventHorizon.Zone.Core.Model.Entity;
    using MediatR;

    public class RunCaptureLogicForAllPlayersHandler : INotificationHandler<RunCaptureLogicForAllPlayers>
    {
        private readonly IMediator _mediator;

        public RunCaptureLogicForAllPlayersHandler(
            IMediator mediator
        )
        {
            this._mediator = mediator;
        }

        public async Task Handle(
            RunCaptureLogicForAllPlayers notification,
            CancellationToken cancellationToken
        )
        {
            var listOfPlayers = await _mediator.Send(
                new QueryForEntities()
                {
                    Query = entity => entity.Type == EntityType.PLAYER,
                }, 
                cancellationToken
            );

            foreach (var player in listOfPlayers)
            {
                await _mediator.Send(
                    new RunCaptureLogicForPlayer(
                        player.Id
                    )
                );
            }
        }
    }
}
