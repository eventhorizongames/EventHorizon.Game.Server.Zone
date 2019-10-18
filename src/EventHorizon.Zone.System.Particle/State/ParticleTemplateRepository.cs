using System.Collections.Generic;
using EventHorizon.Zone.System.Particle.Model.Template;

namespace EventHorizon.Zone.System.Particle.State
{
    public interface ParticleTemplateRepository
    {
        IEnumerable<ParticleTemplate> All();
        void Add(
            string id, 
            ParticleTemplate template
        );
    }
}