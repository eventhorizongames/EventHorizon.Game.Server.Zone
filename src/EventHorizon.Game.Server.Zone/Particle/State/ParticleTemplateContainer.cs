using System.Collections.Concurrent;
using System.Collections.Generic;
using EventHorizon.Zone.Core.Model.Particle;

namespace EventHorizon.Game.Server.Zone.Particle.State
{
    public class ParticleTemplateContainer : ParticleState
    {
        private static readonly ConcurrentDictionary<string, ParticleTemplate> TEMPLATE_MAP = new ConcurrentDictionary<string, ParticleTemplate>();

        public void Add(string id, ParticleTemplate template)
        {
            TEMPLATE_MAP.AddOrUpdate(id, template, (key, old) => template);
        }

        public IEnumerable<ParticleTemplate> All()
        {
            return TEMPLATE_MAP.Values;
        }
    }
}