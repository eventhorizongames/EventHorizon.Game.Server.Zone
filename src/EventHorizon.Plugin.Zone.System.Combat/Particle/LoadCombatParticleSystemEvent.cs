using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Particle.Event
{
    public struct LoadCombatParticleSystemEvent : INotification
    {
        public string FileName { get; set; }
        public LoadCombatParticleSystemEvent(
            string fileName
        ) {
            this.FileName = fileName;
        }
    }
}