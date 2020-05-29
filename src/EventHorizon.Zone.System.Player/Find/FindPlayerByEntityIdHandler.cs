namespace EventHorizon.Zone.System.Player.Find
{
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.Core.Model.Entity.State;
    using EventHorizon.Zone.Core.Model.Player;
    using EventHorizon.Zone.System.Player.Events.Find;
    using global::System.Linq;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;

    public class FindPlayerByEntityIdHandler
        : IRequestHandler<FindPlayerByEntityId, PlayerEntity>
    {
        private readonly EntityRepository _repository;

        public FindPlayerByEntityIdHandler(
            EntityRepository repository
        )
        {
            _repository = repository;
        }

        public async Task<PlayerEntity> Handle(
            FindPlayerByEntityId request,
            CancellationToken cancellationToken
        ) => (await _repository.Where(
            a => a.Type == EntityType.PLAYER && a.Id == request.EntityId
        )).Cast<PlayerEntity>().FirstOrDefault();
    }
}