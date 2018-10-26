using System.Collections.Generic;
using EventHorizon.Game.Server.Zone.Model.Particle;

namespace EventHorizon.Game.Server.Zone.Particle
{
    public interface ParticleState
    {
        IEnumerable<ParticleTemplate> All();
        void Add(string id, ParticleTemplate template);
    }
}