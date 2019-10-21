using System.Threading;
using EventHorizon.Game.Server.Zone;
using EventHorizon.Tests.TestUtils;
using EventHorizon.Zone.System.Server.Scripts.Events.Load;
using EventHorizon.Zone.System.Server.Scripts.Model;
using EventHorizon.Zone.System.Server.Scripts.State;
using EventHorizon.Zone.System.Server.Scripts.System;
using MediatR;
using Moq;
using Xunit;

namespace EventHorizon.Zone.System.Server.Scripts.Tests
{
    public class SystemServerScriptsExtensionsTests
    {
        [Fact]
        public void TestAddServerSetup_ShouldAddExpectedServices()
        {
            // Given
            var serviceCollectionMock = new ServiceCollectionMock();

            // When
            SystemServerScriptsExtensions.AddSystemServerScripts(
                serviceCollectionMock
            );

            // Then
            Assert.Collection(
                serviceCollectionMock,
                service =>
                {
                    Assert.Equal(typeof(ServerScriptRepository), service.ServiceType);
                    Assert.Equal(typeof(ServerScriptInMemoryRepository), service.ImplementationType);
                },
                service =>
                {
                    Assert.Equal(typeof(ServerScriptDetailsRepository), service.ServiceType);
                    Assert.Equal(typeof(ServerScriptDetailsInMemoryRepository), service.ImplementationType);
                },
                service =>
                {
                    Assert.Equal(typeof(ServerScriptServices), service.ServiceType);
                    Assert.Equal(typeof(SystemServerScriptServices), service.ImplementationType);
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
            var actual = SystemServerScriptsExtensions.UseSystemServerScripts(
                applicationBuilderMocks.ApplicationBuilderMock.Object
            );

            // Then
            Assert.Equal(
                expected,
                actual
            );
            mediatorMock.Verify(
                mock => mock.Send(
                    new LoadServerScriptsCommand(),
                    CancellationToken.None
                )
            );
        }
    }
}