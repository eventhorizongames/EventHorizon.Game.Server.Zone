namespace EventHorizon.Zone.System.Particle.State
{
    using EventHorizon.Zone.System.Particle.Model.Template;

    using global::System.Collections.Generic;

    public interface ParticleTemplateRepository
    {
        IEnumerable<ParticleTemplate> All();
        void Add(
            string id,
            ParticleTemplate template
        );
        void Clear();
    }
}
