using System.Numerics;

using EventHorizon.Zone.Core.Model.Graphics;
using EventHorizon.Zone.System.Particle.Model.Settings;
using EventHorizon.Zone.System.Particle.Model.Template;

using Xunit;

namespace EventHorizon.Zone.System.Particle.Tests.Settings
{
    public class ParticleTemplateTests
    {
        [Fact]
        public void TestValidateParticleTemplateKeepsExpectedApi()
        {
            // Given
            var id = "id";
            var name = "name";
            var type = "type";
            var defaultSettings = new ParticleSettings();


            // When
            var actual = new ParticleTemplate
            {
                Id = id,
                Name = name,
                Type = type,
                DefaultSettings = defaultSettings
            };

            // Then
            Assert.Equal(id, actual.Id);
            Assert.Equal(name, actual.Name);
            Assert.Equal(type, actual.Type);
            Assert.Equal(defaultSettings, actual.DefaultSettings);
        }
    }
}
