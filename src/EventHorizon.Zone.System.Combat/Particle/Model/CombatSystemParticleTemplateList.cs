using System.Collections.Generic;
using EventHorizon.Zone.System.Particle.Model.Template;

namespace EventHorizon.Zone.System.Combat.Particle.Model
{
    public struct CombatSystemParticleTemplateList
    {
        public IEnumerable<ParticleTemplate> TemplateList { get; set; }
    }
}