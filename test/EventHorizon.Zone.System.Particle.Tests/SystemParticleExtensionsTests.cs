using EventHorizon.Game.Server.Zone;
using EventHorizon.Tests.TestUtils;
using EventHorizon.Zone.System.Particle.State;
using Xunit;

namespace EventHorizon.Zone.System.Player.Tests
{
    public class SystemParticleExtensionsTests
    {
        [Fact]
        public void TestAddServerSetup_ShouldAddExpectedServices()
        {
            // Given
            var serviceCollectionMock = new ServiceCollectionMock();

            // When
            SystemParticleExtensions.AddSystemParticle(
                serviceCollectionMock
            );

            // Then
            Assert.Collection(
                serviceCollectionMock,
                service =>
                {
                    Assert.Equal(typeof(ParticleTemplateRepository), service.ServiceType);
                    Assert.Equal(typeof(StandardParticleTemplateRepository), service.ImplementationType);
                }
            );
        }

        [Fact]
        public void TestShouldReturnApplicationBuilderForChainingCommands()
        {
            // Given
            var applicationBuilderMocks = ApplicationBuilderFactory.CreateApplicationBuilder();
            var expected = applicationBuilderMocks.ApplicationBuilderMock.Object;

            // When
            var actual = SystemParticleExtensions.UseSystemParticle(
                applicationBuilderMocks.ApplicationBuilderMock.Object
            );

            // Then
            Assert.Equal(
                expected,
                actual
            );
        }
    }
}