namespace EventHorizon.Zone.System.Client.Scripts.Tests
{
    using EventHorizon.Game.Server.Zone;
    using EventHorizon.Test.Common;
    using EventHorizon.Test.Common.Utils;
    using EventHorizon.Zone.System.Client.Scripts.Api;
    using EventHorizon.Zone.System.Client.Scripts.Load;
    using EventHorizon.Zone.System.Client.Scripts.Plugin.Compiler.Api;
    using EventHorizon.Zone.System.Client.Scripts.Plugin.Compiler.Consolidate;
    using EventHorizon.Zone.System.Client.Scripts.Plugin.Compiler.CSharp;
    using EventHorizon.Zone.System.Client.Scripts.State;
    using FluentAssertions;
    using global::System.Threading;
    using MediatR;
    using Moq;
    using Xunit;

    public class SystemClientScriptsExtensionsTests
    {
        [Fact]
        public void ShouldRegisterExpectedServices()
        {
            // Given
            var serviceCollectionMock = new ServiceCollectionMock();

            // When
            SystemClientScriptsExtensions.AddSystemClientScripts(
                serviceCollectionMock
            );

            // Then
            Assert.Collection(
                serviceCollectionMock,
                service =>
                {
                    Assert.Equal(typeof(ClientScriptsState), service.ServiceType);
                    Assert.Equal(typeof(InMemoryClientScriptsState), service.ImplementationType);
                },
                service =>
                {
                    Assert.Equal(typeof(ClientScriptRepository), service.ServiceType);
                    Assert.Equal(typeof(ClientScriptInMemoryRepository), service.ImplementationType);
                },
                service =>
                {
                    Assert.Equal(typeof(ClientScriptsConsolidator), service.ServiceType);
                    Assert.Equal(typeof(StandardClientScriptsConsolidator), service.ImplementationType);
                },
                service =>
                {
                    Assert.Equal(typeof(ClientScriptCompiler), service.ServiceType);
                    Assert.Equal(typeof(ClientScriptCompilerForCSharp), service.ImplementationType);
                }
            );
        }

        [Fact]
        public void TestShouldPublishLoadClientScriptsSystemCommandWhenUseCoreIsCalled()
        {
            // Given
            var applicationBuilderMocks = ApplicationBuilderFactory.CreateApplicationBuilder();
            var expected = new LoadClientScriptsSystemCommand();

            var mediatorMock = new Mock<IMediator>();

            applicationBuilderMocks.ServiceProviderMock.Setup(
                mock => mock.GetService(typeof(IMediator))
            ).Returns(
                mediatorMock.Object
            );

            // When
            var actual = SystemClientScriptsExtensions.UseSystemClientScripts(
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
