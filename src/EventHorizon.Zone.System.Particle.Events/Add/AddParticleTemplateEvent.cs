namespace EventHorizon.Zone.System.Particle.Events.Add
{
    using EventHorizon.Zone.System.Particle.Model.Template;

    using MediatR;

    public struct AddParticleTemplateEvent : INotification
    {
        public string Id { get; }
        public ParticleTemplate Template { get; }

        public AddParticleTemplateEvent(
            string id,
            ParticleTemplate template
        )
        {
            Id = id;
            Template = template;
        }
    }
}
