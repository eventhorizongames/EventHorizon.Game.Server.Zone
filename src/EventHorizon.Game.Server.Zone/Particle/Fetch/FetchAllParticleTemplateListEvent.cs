using System.Collections.Generic;
using EventHorizon.Zone.Core.Model.Particle;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Particle.Fetch
{
    public struct FetchAllParticleTemplateListEvent : IRequest<IEnumerable<ParticleTemplate>>
    {

    }
}