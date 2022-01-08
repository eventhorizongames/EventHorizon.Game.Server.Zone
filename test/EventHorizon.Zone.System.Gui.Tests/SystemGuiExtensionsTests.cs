namespace EventHorizon.Zone.System.Gui.Tests;

using EventHorizon.Game.Server.Zone;
using EventHorizon.Test.Common;
using EventHorizon.Test.Common.Utils;
using EventHorizon.Zone.System.Gui.Api;
using EventHorizon.Zone.System.Gui.Load;
using EventHorizon.Zone.System.Gui.State;

using global::System.Threading;

using MediatR;

using Moq;

using Xunit;

public class SystemGuiExtensionsTests
{
    [Fact]
    public void ShouldAddExpectedServices()
    {
        // Given
        var serviceCollectionMock = new ServiceCollectionMock();

        // When
        var actual = SystemGuiExtensions.AddSystemGui(
            serviceCollectionMock
        );

        // Then
        Assert.Collection(
            actual,
            service =>
            {
                Assert.Equal(typeof(GuiState), service.ServiceType);
                Assert.Equal(typeof(InMemoryGuiState), service.ImplementationType);
            }
        );
    }

    [Fact]
    public void ShouldReturnApplicationBuilderForChainingCommands()
    {
        // Given
        var applicationBuilderMocks = ApplicationBuilderFactory.CreateApplicationBuilder();
        var expected = new LoadSystemGuiCommand();

        var mediatorMock = new Mock<IMediator>();

        applicationBuilderMocks.ServiceProviderMock.Setup(
            mock => mock.GetService(typeof(IMediator))
        ).Returns(
            mediatorMock.Object
        );

        // When
        var actual = SystemGuiExtensions.UseSystemGui(
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
