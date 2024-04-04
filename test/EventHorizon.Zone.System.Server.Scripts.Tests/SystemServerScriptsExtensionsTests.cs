namespace EventHorizon.Zone.System.Server.Scripts.Tests;

using EventHorizon.Game.Server.Zone;
using EventHorizon.Observer.Admin.State;
using EventHorizon.Observer.State;
using EventHorizon.Test.Common;
using EventHorizon.Test.Common.Utils;
using EventHorizon.Zone.System.Server.Scripts.Api;
using EventHorizon.Zone.System.Server.Scripts.Model;
using EventHorizon.Zone.System.Server.Scripts.Parsers;
using EventHorizon.Zone.System.Server.Scripts.State;
using EventHorizon.Zone.System.Server.Scripts.System;

using FluentAssertions;

using global::System;

using Microsoft.Extensions.Logging;

using Moq;

using Xunit;

public class SystemServerScriptsExtensionsTests
{
    [Fact]
    public void TestAddServerSetup_ShouldAddExpectedServices()
    {
        // Given
        var expectedCompilerSubProcessDirectorySetting = "/sub-processes/server-scripts";
        var expectedCompilerSubProcessSetting =
            "EventHorizon.Game.Server.Zone.Server.Scripts.SubProcess";

        var serviceCollectionMock = new ServiceCollectionMock();
        var serviceProviderMock = new Mock<IServiceProvider>();
        var genericObserverStateMock = new Mock<GenericObserverState>(
            It.IsAny<ILogger<GenericObserverState>>()
        );

        serviceProviderMock
            .Setup(mock => mock.GetService(typeof(GenericObserverState)))
            .Returns(genericObserverStateMock.Object);

        // When
        SystemServerScriptsExtensions.AddSystemServerScripts(serviceCollectionMock, options => { });

        // Then
        Assert.Collection(
            serviceCollectionMock,
            service =>
            {
                Assert.Equal(
                    typeof(ServerScriptsSettings),
                    service.ImplementationInstance.GetType()
                );

                if (service.ImplementationInstance is ServerScriptsSettings settings)
                {
                    settings.CompilerSubProcessDirectory
                        .Should()
                        .Be(expectedCompilerSubProcessDirectorySetting);
                    settings.CompilerSubProcess.Should().Be(expectedCompilerSubProcessSetting);
                }
            },
            service =>
            {
                Assert.Equal(typeof(ServerScriptsState), service.ServiceType);
                Assert.Equal(typeof(StandardServerScriptsState), service.ImplementationType);
            },
            service =>
            {
                Assert.Equal(typeof(ServerScriptRepository), service.ServiceType);
                Assert.Equal(typeof(ServerScriptInMemoryRepository), service.ImplementationType);
            },
            service =>
            {
                Assert.Equal(typeof(ServerScriptDetailsRepository), service.ServiceType);
                Assert.Equal(
                    typeof(ServerScriptDetailsInMemoryRepository),
                    service.ImplementationType
                );
            },
            service =>
            {
                Assert.Equal(typeof(ServerScriptMediator), service.ServiceType);
                Assert.Equal(typeof(SystemServerScriptMediator), service.ImplementationType);
            },
            service =>
            {
                Assert.Equal(typeof(ServerScriptObserverBroker), service.ServiceType);
                Assert.Equal(typeof(SystemServerScriptObserverBroker), service.ImplementationType);
            },
            service =>
            {
                Assert.Equal(typeof(DataParsers), service.ServiceType);
                Assert.Equal(typeof(StandardDataParsers), service.ImplementationType);
            },
            service =>
            {
                Assert.Equal(typeof(ServerScriptServices), service.ServiceType);
                Assert.Equal(typeof(SystemServerScriptServices), service.ImplementationType);
            },
            service =>
            {
                Assert.Equal(typeof(GenericObserverState), service.ServiceType);
                Assert.Equal(typeof(GenericObserverState), service.ImplementationType);
            },
            service =>
            {
                service.ServiceType.Should().BeAssignableTo<ObserverState>();
                service
                    .ImplementationFactory(serviceProviderMock.Object)
                    .Should()
                    .BeAssignableTo<GenericObserverState>();
            },
            service =>
            {
                service.ServiceType.Should().BeAssignableTo<AdminObserverState>();
                service
                    .ImplementationFactory(serviceProviderMock.Object)
                    .Should()
                    .BeAssignableTo<GenericObserverState>();
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
        var actual = SystemServerScriptsExtensions.UseSystemServerScripts(
            applicationBuilderMocks.ApplicationBuilderMock.Object
        );

        // Then
        Assert.Equal(expected, actual);
    }
}
