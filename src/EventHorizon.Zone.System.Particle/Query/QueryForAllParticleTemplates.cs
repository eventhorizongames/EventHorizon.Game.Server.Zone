namespace EventHorizon.Zone.System.Particle.Query;

using EventHorizon.Zone.System.Particle.Model.Template;

using global::System.Collections.Generic;

using MediatR;

public struct QueryForAllParticleTemplates
    : IRequest<IEnumerable<ParticleTemplate>> { }
