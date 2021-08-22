namespace EventHorizon.Zone.System.Particle.Fetch
{
    using global::System.Collections.Generic;

    using EventHorizon.Zone.System.Particle.Model.Template;

    using MediatR;

    public struct FetchAllParticleTemplateListEvent : IRequest<IEnumerable<ParticleTemplate>>
    {

    }
}
