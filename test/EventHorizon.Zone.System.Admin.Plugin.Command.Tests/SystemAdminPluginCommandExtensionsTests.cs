using System.Threading;

using EventHorizon.Game.Server.Zone;
using EventHorizon.Tests.TestUtils;
using EventHorizon.Zone.System.Admin.Plugin.Command.Load;
using EventHorizon.Zone.System.Admin.Plugin.Command.State;

using MediatR;

using Moq;

using Xunit;

namespace EventHorizon.Zone.System.Admin.Plugin.Command.Tests
{
    public class SystemAdminPluginCommandExtensionsTests
    {
        [Fact]
        public void TestShouldConfigurationServiceCollection()
        {
            // Given
            var serviceCollectionMock = new ServiceCollectionMock();

            // When
            SystemAdminPluginCommandExtensions.AddSystemAdminPluginCommand(
                serviceCollectionMock
            );

            // Then
            Assert.Collection(
                serviceCollectionMock,
                service =>
                {
                    Assert.Equal(typeof(AdminCommandRepository), service.ServiceType);
                    Assert.Equal(typeof(AdminCommandInMemoryRepository), service.ImplementationType);
                }
            );
        }

        [Fact]
        public void TestShouldConfigurationApplicationBuilder()
        {
            // Given
            var mocks = ApplicationBuilderFactory.CreateApplicationBuilder();
            var expected = new LoadAdminCommands();

            var mediatorMock = new Mock<IMediator>();

            mocks.ServiceProviderMock.Setup(
                mock => mock.GetService(
                    typeof(IMediator)
                )
            ).Returns(
                mediatorMock.Object
            );

            // When
            var actual = SystemAdminPluginCommandExtensions.UseSystemAdminPluginCommand(
                mocks.ApplicationBuilderMock.Object
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
