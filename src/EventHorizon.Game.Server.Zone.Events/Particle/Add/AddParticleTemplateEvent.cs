using EventHorizon.Game.Server.Zone.Model.Particle;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Events.Particle.Add
{
    public struct AddParticleTemplateEvent : INotification
    {
        public string Id { get; set; }
        public ParticleTemplate Template { get; set; }
    }
}