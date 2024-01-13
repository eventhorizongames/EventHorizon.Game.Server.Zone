namespace EventHorizon.Zone.System.Client.Scripts.Tests;

using EventHorizon.Game.Server.Zone;
using EventHorizon.Test.Common;
using EventHorizon.Test.Common.Utils;
using EventHorizon.Zone.System.Client.Scripts.Api;
using EventHorizon.Zone.System.Client.Scripts.Load;
using EventHorizon.Zone.System.Client.Scripts.Model;
using EventHorizon.Zone.System.Client.Scripts.State;
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
        SystemClientScriptsExtensions.AddSystemClientScripts(serviceCollectionMock, _ => { });

        // Then
        Assert.Collection(
            serviceCollectionMock,
            service =>
            {
                Assert.Equal(typeof(ClientScriptsSettings), service.ServiceType);
            },
            service =>
            {
                Assert.Equal(typeof(ClientScriptsState), service.ServiceType);
                Assert.Equal(typeof(InMemoryClientScriptsState), service.ImplementationType);
            },
            service =>
            {
                Assert.Equal(typeof(ClientScriptRepository), service.ServiceType);
                Assert.Equal(
                    typeof(ClientScriptInMemoryRepository),
                    service.ImplementationType
                );
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

        applicationBuilderMocks
            .ServiceProviderMock.Setup(mock => mock.GetService(typeof(IMediator)))
            .Returns(mediatorMock.Object);

        // When
        var actual = SystemClientScriptsExtensions.UseSystemClientScripts(
            applicationBuilderMocks.ApplicationBuilderMock.Object
        );

        // Then
        mediatorMock.Verify(mock => mock.Send<IRequest>(expected, CancellationToken.None));
    }
}
