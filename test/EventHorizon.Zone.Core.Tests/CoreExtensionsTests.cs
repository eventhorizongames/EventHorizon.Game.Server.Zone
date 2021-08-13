namespace EventHorizon.Zone.Core.Tests
{
    using System.Reflection;
    using System.Threading;
    using EventHorizon.Game.Server.Zone;
    using EventHorizon.Monitoring.Events.Track;
    using EventHorizon.Tests.TestUtils;
    using EventHorizon.Zone.Core.DateTimeService;
    using EventHorizon.Zone.Core.Events.Lifetime;
    using EventHorizon.Zone.Core.Id;
    using EventHorizon.Zone.Core.Info;
    using EventHorizon.Zone.Core.Json;
    using EventHorizon.Zone.Core.Lifetime.State;
    using EventHorizon.Zone.Core.Model.DateTimeService;
    using EventHorizon.Zone.Core.Model.DirectoryService;
    using EventHorizon.Zone.Core.Model.FileService;
    using EventHorizon.Zone.Core.Model.Id;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.Core.Model.Json;
    using EventHorizon.Zone.Core.Model.RandomNumber;
    using EventHorizon.Zone.Core.Model.ServerProperty;
    using EventHorizon.Zone.Core.Plugin.LocalFileSystem;
    using EventHorizon.Zone.Core.RandomNumber;
    using EventHorizon.Zone.Core.ServerProperty;
    using EventHorizon.Zone.Core.ServerProperty.Fill;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;
    using static EventHorizon.Game.Server.Zone.CoreExtensions;

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
                    Assert.Equal(typeof(LocalFileSystemDirectoryResolver), service.ImplementationType);
                },
                service =>
                {
                    Assert.Equal(typeof(FileResolver), service.ServiceType);
                    Assert.Equal(typeof(LocalFileSystemFileResolver), service.ImplementationType);
                },
                service =>
                {
                    Assert.Equal(typeof(IdPool), service.ServiceType);
                    Assert.Equal(typeof(InMemoryStaticIdPool), service.ImplementationType);
                },
                service =>
                {
                    Assert.Equal(typeof(ServerInfo), service.ServiceType);
                    Assert.Equal(typeof(HostEnvironmentServerInfo), service.ImplementationType);
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
                    Assert.Equal(typeof(ServerLifetimeState), service.ServiceType);
                    Assert.Equal(typeof(StandardServerLifetimeState), service.ImplementationType);
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

        [Fact]
        public void TestShouldPublishMonitoringTrackEventWhenUseStartingCoreIsCalled()
        {
            // Given
            var applicationBuilderMocks = ApplicationBuilderFactory.CreateApplicationBuilder();
            var expected = new MonitoringTrackEvent(
                "ZoneServer:Starting"
            );

            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<CoreStartup>>();

            applicationBuilderMocks.ServiceProviderMock.Setup(
                mock => mock.GetService(typeof(IMediator))
            ).Returns(
                mediatorMock.Object
            );
            applicationBuilderMocks.ServiceProviderMock.Setup(
                mock => mock.GetService(typeof(ILogger<CoreStartup>))
            ).Returns(
                loggerMock.Object
            );

            // When
            var actual = CoreExtensions.UseStartingCore(
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

        [Fact]
        public void ShouldSendFinishServerStartCommandWhenUseFinishStartingCoreIsCalled()
        {
            // Given
            var applicationBuilderMocks = ApplicationBuilderFactory.CreateApplicationBuilder();
            var expected = new FinishServerStartCommand();

            var mediatorMock = new Mock<IMediator>();

            applicationBuilderMocks.ServiceProviderMock.Setup(
                mock => mock.GetService(typeof(IMediator))
            ).Returns(
                mediatorMock.Object
            );

            // When
            var actual = CoreExtensions.UseFinishStartingCore(
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
