using System.Reflection;
using System.Threading;
using EventHorizon.Game.Server.Zone;
using EventHorizon.Tests.TestUtils;
using EventHorizon.Zone.Core.DateTimeService;
using EventHorizon.Zone.Core.DirectoryService;
using EventHorizon.Zone.Core.Id;
using EventHorizon.Zone.Core.Info;
using EventHorizon.Zone.Core.Json;
using EventHorizon.Zone.Core.Model.DateTimeService;
using EventHorizon.Zone.Core.Model.DirectoryService;
using EventHorizon.Zone.Core.Model.Id;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.Core.Model.Json;
using EventHorizon.Zone.Core.Model.RandomNumber;
using EventHorizon.Zone.Core.Model.ServerProperty;
using EventHorizon.Zone.Core.RandomNumber;
using EventHorizon.Zone.Core.ServerProperty;
using EventHorizon.Zone.Core.ServerProperty.Fill;
using MediatR;
using Moq;
using Xunit;

namespace EventHorizon.Zone.Core.Tests
{
    public class CoreExtensionsTests
    {
        [Fact]
        public void TestShouldConfigureServiceCollectionWithExpectedServices()
        {
            // Given
            var systemProvidedAssemblyList = new Assembly[0];
            var serviceCollectionMock = new ServiceCollectionMock();

            // When
            CoreExtensions.AddCore(
                serviceCollectionMock,
                systemProvidedAssemblyList
            );

            var actual = serviceCollectionMock.Services.Values;

            // Then
            Assert.Collection(
                actual,
                service =>
                {
                    Assert.Equal(typeof(IDateTimeService), service.ServiceType);
                    Assert.Equal(typeof(StandardDateTimeService), service.ImplementationType);
                },
                service =>
                {
                    Assert.Equal(typeof(DirectoryResolver), service.ServiceType);
                    Assert.Equal(typeof(StandardDirectoryResolver), service.ImplementationType);
                },
                service =>
                {
                    Assert.Equal(typeof(IdPool), service.ServiceType);
                    Assert.Equal(typeof(InMemoryStaticIdPool), service.ImplementationType);
                },
                service =>
                {
                    Assert.Equal(typeof(ServerInfo), service.ServiceType);
                    Assert.Equal(typeof(HostingEnvironmentServerInfo), service.ImplementationType);
                },
                service =>
                {
                    Assert.Equal(typeof(SystemProvidedAssemblyList), service.ServiceType);
                    Assert.Equal(typeof(StandardSystemProvidedAssemblyList), service.ImplementationInstance.GetType());
                },
                service =>
                {
                    Assert.Equal(typeof(IJsonFileLoader), service.ServiceType);
                    Assert.Equal(typeof(NewtonsoftJsonFileLoader), service.ImplementationType);
                },
                service =>
                {
                    Assert.Equal(typeof(IJsonFileSaver), service.ServiceType);
                    Assert.Equal(typeof(NewtonsoftJsonFileSaver), service.ImplementationType);
                },
                service =>
                {
                    Assert.Equal(typeof(IRandomNumberGenerator), service.ServiceType);
                    Assert.Equal(typeof(CryptographyRandomNumberGenerator), service.ImplementationType);
                },
                service =>
                {
                    Assert.Equal(typeof(IServerProperty), service.ServiceType);
                    Assert.Equal(typeof(InMemoryServerProperty), service.ImplementationType);
                }
            );
        }

        [Fact]
        public void TestShouldPublishFillServerPropertiesEventWhenUseCoreIsCalled()
        {
            // Given
            var applicationBuilderMocks = ApplicationBuilderFactory.CreateApplicationBuilder();
            var expected = new FillServerPropertiesEvent();

            var mediatorMock = new Mock<IMediator>();

            applicationBuilderMocks.ServiceProviderMock.Setup(
                mock => mock.GetService(typeof(IMediator))
            ).Returns(
                mediatorMock.Object
            );

            // When
            var actual = CoreExtensions.UseCore(
                applicationBuilderMocks.ApplicationBuilderMock.Object
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