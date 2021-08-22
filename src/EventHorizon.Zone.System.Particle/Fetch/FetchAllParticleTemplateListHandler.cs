namespace EventHorizon.Zone.System.Particle.Fetch
{
    using global::System.Collections.Generic;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using EventHorizon.Zone.System.Particle.Model.Template;
    using EventHorizon.Zone.System.Particle.State;

    using MediatR;

    public class FetchAllParticleTemplateListHandler : IRequestHandler<FetchAllParticleTemplateListEvent, IEnumerable<ParticleTemplate>>
    {
        readonly ParticleTemplateRepository _repository;
        public FetchAllParticleTemplateListHandler(
            ParticleTemplateRepository repository
        )
        {
            _repository = repository;
        }
        public Task<IEnumerable<ParticleTemplate>> Handle(
            FetchAllParticleTemplateListEvent request,
            CancellationToken cancellationToken
        )
        {
            return Task.FromResult(
                _repository.All()
            );
        }
    }
}
