namespace EventHorizon.Zone.System.Client.Scripts.Tests
{
    using EventHorizon.Game.Server.Zone;
    using EventHorizon.Test.Common;
    using EventHorizon.Test.Common.Utils;
    using EventHorizon.Zone.System.Client.Scripts.Plugin.Compiler.Api;
    using EventHorizon.Zone.System.Client.Scripts.Plugin.Compiler.Builders;
    using EventHorizon.Zone.System.Client.Scripts.Plugin.Compiler.Logging;
    using EventHorizon.Zone.System.Client.Scripts.Plugin.Compiler.Model;
    using FluentAssertions;
    using global::System;
    using global::System.Collections.Generic;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Weikio.PluginFramework.Abstractions;
    using Weikio.PluginFramework.AspNetCore;
    using Weikio.PluginFramework.Catalogs;
    using Xunit;

    public class SystemClientScriptsPluginComplierExtensionsTests
    {
        [Fact]
        public void ShouldRegisterExpectedServices()
        {
            // Given
            Action<ClientScriptsPluginCompilerOptions> optionsAction = options =>
            {
            };

            var serviceCollectionMock = new ServiceCollectionMock();

            // When
            SystemClientScriptsPluginCompilerExtensions.AddSystemClientScriptsPluginCompiler(
                serviceCollectionMock,
                optionsAction
            );

            // Then
            Assert.Collection(
                serviceCollectionMock,
                service =>
                {
                    Assert.Equal(typeof(AssemblyBuilder), service.ServiceType);
                    Assert.Equal(typeof(CSharpAssemblyBuilder), service.ImplementationType);
                },
                service =>
                {
                    Assert.Equal(typeof(IHostedService), service.ServiceType);
                    Assert.Equal(typeof(PluginFrameworkInitializer), service.ImplementationType);
                },
                service =>
                {
                    Assert.Equal(typeof(IEnumerable<Plugin>), service.ServiceType);
                },
                service =>
                {
                    Assert.Equal(typeof(IPluginCatalog), service.ServiceType);
                    Assert.Equal(typeof(NugetPackagePluginCatalog), service.ImplementationInstance.GetType());
                }
            );
        }

        [Fact]
        public void ShouldRegisterDefaultLoggerWhenAddIsCalled()
        {
            // Given
            Action<ClientScriptsPluginCompilerOptions> optionsAction = options => { };

            var serviceCollection = new ServiceCollection();

            var loggerMock = new Mock<ILogger<CompilerPackageLoaderNuGetLogger>>();

            serviceCollection.AddSingleton(
                loggerMock.Object
            );

            // When
            SystemClientScriptsPluginCompilerExtensions.AddSystemClientScriptsPluginCompiler(
                serviceCollection,
                optionsAction
            );
            var actual = NugetPluginCatalogOptions.Defaults.LoggerFactory();

            // Then
            actual.Should().BeOfType(
                typeof(CompilerPackageLoaderNuGetLogger)
            );
        }

        [Fact]
        public void TestShouldPublishLoadClientScriptsSystemCommandWhenUseCoreIsCalled()
        {
            // Given
            var applicationBuilderMocks = ApplicationBuilderFactory.CreateApplicationBuilder();
            var expected = applicationBuilderMocks.ApplicationBuilderMock.Object;

            // When
            var actual = SystemClientScriptsPluginCompilerExtensions.UseSystemClientScriptsPluginCompiler(
                applicationBuilderMocks.ApplicationBuilderMock.Object
            );

            // Then
            actual.Should().Be(
                expected
            );
        }
    }
}
