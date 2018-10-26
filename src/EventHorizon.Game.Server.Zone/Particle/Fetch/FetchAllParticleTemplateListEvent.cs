using System.Collections.Generic;
using EventHorizon.Game.Server.Zone.Model.Particle;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Particle.Fetch
{
    public struct FetchAllParticleTemplateListEvent : IRequest<IEnumerable<ParticleTemplate>>
    {

    }
}