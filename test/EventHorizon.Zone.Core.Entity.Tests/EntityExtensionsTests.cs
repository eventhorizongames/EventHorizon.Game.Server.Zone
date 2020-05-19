namespace EventHorizon.Zone.Core.Entity.Tests
{
    using Xunit;
    using EventHorizon.Game.Server.Zone;
    using EventHorizon.Zone.Core.Entity.State;
    using EventHorizon.Zone.Core.Model.Entity.State;
    using EventHorizon.Test.Common.Utils;

    public class EntityExtensionsTests
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
    }
}