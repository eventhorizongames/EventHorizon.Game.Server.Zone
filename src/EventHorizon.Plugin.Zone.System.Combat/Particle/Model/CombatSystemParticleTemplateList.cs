using System.Collections.Generic;
using EventHorizon.Game.Server.Zone.Model.Particle;

namespace EventHorizon.Plugin.Zone.System.Combat.Particle.Model
{
    public struct CombatSystemParticleTemplateList
    {
        public IEnumerable<ParticleTemplate> TemplateList { get; set; }
    }
}