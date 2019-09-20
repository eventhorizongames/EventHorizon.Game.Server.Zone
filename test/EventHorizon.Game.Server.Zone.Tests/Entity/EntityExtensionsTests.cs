using Xunit;
using Moq;
using EventHorizon.Game.Server.Zone.Entity;
using EventHorizon.Game.Server.Zone.Tests.TestUtil;
using EventHorizon.Game.Server.Zone.Entity.State;
using EventHorizon.Game.Server.Zone.Entity.State.Impl;
using EventHorizon.Zone.Core.Model.Entity;

namespace EventHorizon.Game.Server.Zone.Tests.Entity
{
    public class EntityExtensionsTests
    {
        [Fact]
        public void TestAddEntity_ShouldConfigurationServiceCollection()
        {
            // Given
            var serviceCollectionMock = new ServiceCollectionMock();
            
            // When
            EntityExtensions.AddEntity(serviceCollectionMock);
            
            // Then
            Assert.NotEmpty(serviceCollectionMock);
            Assert.Collection(serviceCollectionMock,
                service =>
                {
                    Assert.Equal(typeof(IEntityRepository), service.ServiceType);
                    Assert.Equal(typeof(EntityRepository), service.ImplementationType);
                },
                service =>
                {
                    Assert.Equal(typeof(IEntitySearchTree), service.ServiceType);
                    Assert.Equal(typeof(EntitySearchTree), service.ImplementationType);
                }
            );
        }
    }
}