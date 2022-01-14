namespace EventHorizon.Zone.System.Particle.Model.Template;

using EventHorizon.Zone.System.Particle.Model.Settings;

public struct ParticleTemplate
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public ParticleSettings DefaultSettings { get; set; }
}
