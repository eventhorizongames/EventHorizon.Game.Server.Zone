namespace EventHorizon.Game.Server.Zone.Model.Particle
{
    public struct ParticleTemplate
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public ParticleSettings DefaultSettings { get; set; }
    }
}