namespace EventHorizon.Zone.System.DataStorage.Tests
{
    using EventHorizon.Game.Server.Zone;
    using EventHorizon.Test.Common;
    using EventHorizon.Test.Common.Utils;
    using EventHorizon.Zone.System.DataStorage.Model;
    using EventHorizon.Zone.System.DataStorage.Provider;
    using global::System;
    using Moq;
    using Xunit;

    public class SystemDataStorageExtensionsTests
    {
        [Fact]
        public void TestAddServerSetup_ShouldAddExpectedServices()
        {
            // Given
            var serviceCollectionMock = new ServiceCollectionMock();
            var serviceProviderMock = new Mock<IServiceProvider>();

            // When
            SystemDataStorageExtensions.AddSystemDataStorage(
                serviceCollectionMock
            );

            // Then
            Assert.Collection(
                serviceCollectionMock,
                service =>
                {
                    Assert.Equal(typeof(DataStore), service.ServiceType);
                    Assert.Equal(typeof(StandardDataStoreProvider), service.ImplementationType);
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
            var actual = SystemDataStorageExtensions.UseSystemDataStorage(
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