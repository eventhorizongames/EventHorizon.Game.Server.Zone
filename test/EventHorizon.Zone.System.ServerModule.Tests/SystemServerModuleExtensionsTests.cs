namespace EventHorizon.Zone.System.ServerModule.Tests
{
    using EventHorizon.Game.Server.Zone;
    using EventHorizon.Test.Common;
    using EventHorizon.Test.Common.Utils;
    using EventHorizon.Zone.System.ServerModule.Load;
    using EventHorizon.Zone.System.ServerModule.State;

    using global::System.Threading;

    using MediatR;

    using Moq;

    using Xunit;

    public class SystemServerModuleExtensionsTests
    {
        [Fact]
        public void TestAddEntity_ShouldConfigurationServiceCollection()
        {
            // Given
            var serviceCollectionMock = new ServiceCollectionMock();

            // When
            SystemServerModuleExtensions.AddSystemServerModule(
                serviceCollectionMock
            );

            // Then
            Assert.Collection(
                serviceCollectionMock,
                service =>
                {
                    Assert.Equal(typeof(ServerModuleRepository), service.ServiceType);
                    Assert.Equal(typeof(ServerModuleInMemoryRepository), service.ImplementationType);
                }
            );
        }

        [Fact]
        public void ShouldPublishEventsWhenCalled()
        {
            // Given
            var expected = new LoadServerModuleSystem();
            var mocks = ApplicationBuilderFactory.CreateApplicationBuilder();

            var mediatorMock = new Mock<IMediator>();

            mocks.ServiceProviderMock.Setup(
                mock => mock.GetService(
                    typeof(IMediator)
                )
            ).Returns(
                mediatorMock.Object
            );

            // When
            var actual = SystemServerModuleExtensions.UseSystemServerModule(
                mocks.ApplicationBuilderMock.Object
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Publish(
                    expected,
                    CancellationToken.None
                )
            );
        }
    }
}
