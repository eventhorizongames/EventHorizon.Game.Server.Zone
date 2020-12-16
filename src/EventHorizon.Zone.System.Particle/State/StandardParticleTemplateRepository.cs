namespace EventHorizon.Zone.System.Particle.State
{
    using global::System.Collections.Concurrent;
    using global::System.Collections.Generic;
    using EventHorizon.Zone.System.Particle.Model.Template;

    public class StandardParticleTemplateRepository
        : ParticleTemplateRepository
    {
        private readonly ConcurrentDictionary<string, ParticleTemplate> _map = new ConcurrentDictionary<string, ParticleTemplate>();

        public void Add(
            string id,
            ParticleTemplate template
        )
        {
            _map.AddOrUpdate(
                id,
                template,
                (_, __) => template
            );
        }

        public IEnumerable<ParticleTemplate> All()
        {
            return _map.Values;
        }

        public void Clear()
        {
            _map.Clear();
        }
    }
}