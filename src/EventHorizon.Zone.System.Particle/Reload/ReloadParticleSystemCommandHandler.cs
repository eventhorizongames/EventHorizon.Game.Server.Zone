namespace EventHorizon.Zone.System.Particle.Reload
{
    using EventHorizon.Zone.Core.Model.Command;
    using EventHorizon.Zone.System.Particle.Load;
    using EventHorizon.Zone.System.Particle.State;

    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    public class ReloadParticleSystemCommandHandler
        : IRequestHandler<ReloadParticleSystemCommand, StandardCommandResult>
    {
        private readonly IMediator _mediator;
        private readonly ParticleTemplateRepository _repository;

        public ReloadParticleSystemCommandHandler(
            IMediator mediator,
            ParticleTemplateRepository repository
        )
        {
            _mediator = mediator;
            _repository = repository;
        }

        public async Task<StandardCommandResult> Handle(
            ReloadParticleSystemCommand request,
            CancellationToken cancellationToken
        )
        {
            _repository.Clear();

            await _mediator.Publish(
                new LoadParticleSystemEvent(),
                cancellationToken
            );

            return new StandardCommandResult();
        }
    }
}
