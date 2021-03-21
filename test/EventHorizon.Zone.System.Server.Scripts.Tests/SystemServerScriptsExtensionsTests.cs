namespace EventHorizon.Zone.System.Server.Scripts.Tests
{
    using EventHorizon.Game.Server.Zone;
    using EventHorizon.Observer.Admin.State;
    using EventHorizon.Observer.State;
    using EventHorizon.Test.Common;
    using EventHorizon.Test.Common.Utils;
    using EventHorizon.Zone.System.Server.Scripts.Api;
    using EventHorizon.Zone.System.Server.Scripts.Model;
    using EventHorizon.Zone.System.Server.Scripts.State;
    using EventHorizon.Zone.System.Server.Scripts.System;
    using FluentAssertions;
    using Xunit;

    public class SystemServerScriptsExtensionsTests
    {
        [Fact]
        public void TestAddServerSetup_ShouldAddExpectedServices()
        {
            // Given
            var expectedCompilerSubProcessDirectorySetting = "/sub-processes/server-scripts";
            var expectedCompilerSubProcessSetting = "EventHorizon.Game.Server.Zone.Server.Scripts.SubProcess";
            var serviceCollectionMock = new ServiceCollectionMock();

            // When
            SystemServerScriptsExtensions.AddSystemServerScripts(
                serviceCollectionMock,
                options => { }
            );

            // Then
            Assert.Collection(
                serviceCollectionMock,
                service =>
                {
                    Assert.Equal(typeof(ServerScriptsSettings), service.ImplementationInstance.GetType());

                    if (service.ImplementationInstance is ServerScriptsSettings settings)
                    {
                        settings.CompilerSubProcessDirectory
                            .Should().Be(expectedCompilerSubProcessDirectorySetting);
                        settings.CompilerSubProcess
                            .Should().Be(expectedCompilerSubProcessSetting);
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
                    Assert.Equal(typeof(ServerScriptDetailsInMemoryRepository), service.ImplementationType);
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
                    Assert.Equal(typeof(ObserverState), service.ServiceType);
                },
                service =>
                {
                    Assert.Equal(typeof(AdminObserverState), service.ServiceType);
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
            Assert.Equal(
                expected,
                actual
            );
        }
    }
}