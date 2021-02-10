namespace EventHorizon.Zone.System.Client.Scripts.Tests
{
    using EventHorizon.Game.Server.Zone;
    using EventHorizon.Test.Common;
    using EventHorizon.Test.Common.Utils;
    using EventHorizon.Zone.System.Server.Scripts.Plugin.Compiler.Api;
    using EventHorizon.Zone.System.Server.Scripts.Plugin.Compiler.Builders;
    using EventHorizon.Zone.System.Server.Scripts.Plugin.Compiler.CSharp;
    using FluentAssertions;
    using global::System;
    using global::System.Collections.Generic;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    public class SystemServerScriptsPluginComplierExtensionsTests
    {
        [Fact]
        public void ShouldRegisterExpectedServices()
        {
            // Given
            var serviceCollectionMock = new ServiceCollectionMock();

            // When
            SystemServerScriptsPluginCompilerExtensions.AddSystemServerScriptsPluginCompiler(
                serviceCollectionMock
            );

            // Then
            serviceCollectionMock.Should().Contain(
                service => typeof(AssemblyBuilder) == service.ServiceType
                    && typeof(CSharpAssemblyBuilder) == service.ImplementationType
            ).And.Contain(
                service => typeof(ServerScriptCompiler) == service.ServiceType
                    && typeof(ServerScriptCompilerForCSharp) == service.ImplementationType
            );
        }

        [Fact]
        public void TestShouldPublishLoadClientScriptsSystemCommandWhenUseCoreIsCalled()
        {
            // Given
            var applicationBuilderMocks = ApplicationBuilderFactory.CreateApplicationBuilder();
            var expected = applicationBuilderMocks.ApplicationBuilderMock.Object;

            // When
            var actual = SystemServerScriptsPluginCompilerExtensions.UseSystemClientScriptsPluginCompiler(
                applicationBuilderMocks.ApplicationBuilderMock.Object
            );

            // Then
            actual.Should().Be(
                expected
            );
        }
    }
}
