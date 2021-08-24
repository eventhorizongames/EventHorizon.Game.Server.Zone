namespace EventHorizon.Zone.System.Particle.State
{
    using EventHorizon.Zone.System.Particle.Model.Template;

    using global::System.Collections.Concurrent;
    using global::System.Collections.Generic;

    public class StandardParticleTemplateRepository
        : ParticleTemplateRepository
    {
        private readonly ConcurrentDictionary<string, ParticleTemplate> _map = new();

        public void Add(
            string id,
            ParticleTemplate template
        )
        {
            _map.AddOrUpdate(
                id,
                template,
                (_, _) => template
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
