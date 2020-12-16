namespace EventHorizon.Zone.System.Particle.State
{
    using global::System.Collections.Generic;
    using EventHorizon.Zone.System.Particle.Model.Template;

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