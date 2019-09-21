using System.Collections.Generic;
using EventHorizon.Zone.Core.Model.Particle;

namespace EventHorizon.Zone.System.Combat.Particle.Model
{
    public struct CombatSystemParticleTemplateList
    {
        public IEnumerable<ParticleTemplate> TemplateList { get; set; }
    }
}