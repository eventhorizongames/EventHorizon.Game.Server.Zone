using EventHorizon.Zone.Core.Model.Particle;
using MediatR;

namespace EventHorizon.Zone.Core.Events.Particle.Add
{
    public struct AddParticleTemplateEvent : INotification
    {
        public string Id { get; set; }
        public ParticleTemplate Template { get; set; }
    }
}