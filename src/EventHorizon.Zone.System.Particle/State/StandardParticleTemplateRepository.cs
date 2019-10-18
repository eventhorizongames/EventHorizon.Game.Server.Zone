using System.Collections.Concurrent;
using System.Collections.Generic;
using EventHorizon.Zone.System.Particle.Model.Template;

namespace EventHorizon.Zone.System.Particle.State
{
    public class StandardParticleTemplateRepository : ParticleTemplateRepository
    {
        private readonly ConcurrentDictionary<string, ParticleTemplate> TEMPLATE_MAP = new ConcurrentDictionary<string, ParticleTemplate>();

        public void Add(
            string id, 
            ParticleTemplate template
        )
        {
            TEMPLATE_MAP.AddOrUpdate(
                id, 
                template, 
                (key, old) => template
            );
        }

        public IEnumerable<ParticleTemplate> All()
        {
            return TEMPLATE_MAP.Values;
        }
    }
}