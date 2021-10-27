namespace EventHorizon.Zone.Core.Entity.Tests
{
    using EventHorizon.Game.Server.Zone;
    using EventHorizon.Test.Common;
    using EventHorizon.Test.Common.Utils;
    using EventHorizon.Zone.Core.Entity.Load;
    using EventHorizon.Zone.Core.Entity.State;
    using EventHorizon.Zone.Core.Model.Entity.State;

    using global::System.Threading;

    using MediatR;

    using Moq;

    using Xunit;

    public class CoreEntityExtensionsTests
    {
        [Fact]
        public void TestAddEntity_ShouldConfigurationServiceCollection()
        {
            // Given
            var serviceCollectionMock = new ServiceCollectionMock();

            // When
            CoreEntityExtensions.AddCoreEntity(
                serviceCollectionMock
            );

            // Then
            Assert.Collection(
                serviceCollectionMock,
                service =>
                {
                    Assert.Equal(typeof(EntityRepository), service.ServiceType);
                    Assert.Equal(typeof(InMemoryEntityRepository), service.ImplementationType);
                },
                service =>
                {
                    Assert.Equal(typeof(EntitySearchTree), service.ServiceType);
                    Assert.Equal(typeof(InMemoryEntitySearchTree), service.ImplementationType);
                }
            );
        }

        [Fact]
        public void ShouldReturnApplicationBuilderForChainingCommands()
        {
            // Given
            var applicationBuilderMocks = ApplicationBuilderFactory.CreateApplicationBuilder();
            var expected = new LoadEntityCoreCommand();

            var mediatorMock = new Mock<IMediator>();

            applicationBuilderMocks.ServiceProviderMock.Setup(
                mock => mock.GetService(typeof(IMediator))
            ).Returns(
                mediatorMock.Object
            );

            // When
            var actual = CoreEntityExtensions.UseCoreEntity(
                applicationBuilderMocks.ApplicationBuilderMock.Object
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Send(
                    expected,
                    CancellationToken.None
                )
            );
        }
    }
}
