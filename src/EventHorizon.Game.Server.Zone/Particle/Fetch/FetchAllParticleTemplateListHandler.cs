using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Model.Particle;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Particle.Fetch
{
    public class FetchAllParticleTemplateListHandler : IRequestHandler<FetchAllParticleTemplateListEvent, IEnumerable<ParticleTemplate>>
    {
        readonly ParticleState _particleState;
        public FetchAllParticleTemplateListHandler(
            ParticleState particleState
        )
        {
            _particleState = particleState;
        }
        public Task<IEnumerable<ParticleTemplate>> Handle(FetchAllParticleTemplateListEvent request, CancellationToken cancellationToken)
        {
            return Task.FromResult(
                _particleState.All()
            );
        }
    }
}