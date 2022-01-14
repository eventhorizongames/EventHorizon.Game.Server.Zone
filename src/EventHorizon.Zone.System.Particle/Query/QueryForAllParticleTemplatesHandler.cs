namespace EventHorizon.Zone.System.Particle.Query;

using EventHorizon.Zone.System.Particle.Model.Template;
using EventHorizon.Zone.System.Particle.State;

using global::System.Collections.Generic;
using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

public class QueryForAllParticleTemplatesHandler
    : IRequestHandler<
          QueryForAllParticleTemplates,
          IEnumerable<ParticleTemplate>
      >
{
    readonly ParticleTemplateRepository _repository;

    public QueryForAllParticleTemplatesHandler(
        ParticleTemplateRepository repository
    )
    {
        _repository = repository;
    }

    public Task<IEnumerable<ParticleTemplate>> Handle(
        QueryForAllParticleTemplates request,
        CancellationToken cancellationToken
    )
    {
        return _repository.All().FromResult();
    }
}
