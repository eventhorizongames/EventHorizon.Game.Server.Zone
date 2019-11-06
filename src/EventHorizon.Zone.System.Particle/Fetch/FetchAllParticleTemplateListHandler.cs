using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Particle.Model.Template;
using EventHorizon.Zone.System.Particle.State;
using MediatR;

namespace EventHorizon.Zone.System.Particle.Fetch
{
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