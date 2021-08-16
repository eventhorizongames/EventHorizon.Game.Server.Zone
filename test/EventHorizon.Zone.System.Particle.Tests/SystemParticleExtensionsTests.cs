namespace EventHorizon.Zone.System.Player.Tests
{
    using EventHorizon.Game.Server.Zone;
    using EventHorizon.Tests.TestUtils;
    using EventHorizon.Zone.System.Particle.Load;
    using EventHorizon.Zone.System.Particle.State;

    using global::System.Threading;

    using MediatR;

    using Moq;

    using Xunit;

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

            var mediatorMock = new Mock<IMediator>();

            applicationBuilderMocks.ServiceProviderMock.Setup(
                mock => mock.GetService(typeof(IMediator))
            ).Returns(
                mediatorMock.Object
            );

            // When
            var actual = SystemParticleExtensions.UseSystemParticle(
                applicationBuilderMocks.ApplicationBuilderMock.Object
            );

            // Then
            Assert.Equal(
                expected,
                actual
            );

            mediatorMock.Verify(
                mock => mock.Publish(
                    It.IsAny<LoadParticleSystemEvent>(),
                    CancellationToken.None
                )
            );
        }
    }
}
