namespace EventHorizon.Zone.System.Client.Scripts.Tests
{
    using EventHorizon.Game.Server.Zone;
    using EventHorizon.Test.Common;
    using EventHorizon.Test.Common.Utils;
    using EventHorizon.Zone.System.Client.Scripts.Plugin.Compiler.Api;
    using EventHorizon.Zone.System.Client.Scripts.Plugin.Compiler.Builders;
    using EventHorizon.Zone.System.Client.Scripts.Plugin.Compiler.CSharp;
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
    using Weikio.PluginFramework.Catalogs.NuGet;

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
            serviceCollectionMock.Should().Contain(
                service => typeof(AssemblyBuilder) == service.ServiceType
                    && typeof(CSharpAssemblyBuilder) == service.ImplementationType
            ).And.Contain(
                service => typeof(ClientScriptCompiler) == service.ServiceType
                    && typeof(ClientScriptCompilerForCSharp) == service.ImplementationType
            ).And.Contain(
                service => typeof(IHostedService) == service.ServiceType
                    && typeof(PluginFrameworkInitializer) == service.ImplementationType
            ).And.Contain(
                service => typeof(IEnumerable<Plugin>) == service.ServiceType
            ).And.Contain(
                service => typeof(IPluginCatalog) == service.ServiceType
                    && service.ImplementationInstance != null
                    && typeof(NugetPackagePluginCatalog) == service.ImplementationInstance.GetType()
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
