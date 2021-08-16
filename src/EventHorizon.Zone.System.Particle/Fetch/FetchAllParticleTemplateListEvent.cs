using System.Collections.Generic;

using EventHorizon.Zone.System.Particle.Model.Template;

using MediatR;

namespace EventHorizon.Zone.System.Particle.Fetch
{
    public struct FetchAllParticleTemplateListEvent : IRequest<IEnumerable<ParticleTemplate>>
    {

    }
}
