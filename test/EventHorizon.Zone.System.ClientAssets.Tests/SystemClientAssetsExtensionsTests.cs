namespace EventHorizon.Zone.System.ClientAssets.Tests
{
    using AutoFixture.Xunit2;

    using EventHorizon.Game.Server.Zone;
    using EventHorizon.Test.Common;
    using EventHorizon.Test.Common.Attributes;
    using EventHorizon.Test.Common.Utils;
    using EventHorizon.Zone.System.ClientAssets.Load;
    using EventHorizon.Zone.System.ClientAssets.State;
    using EventHorizon.Zone.System.ClientAssets.State.Api;

    using FluentAssertions;

    using global::System.Threading;

    using MediatR;

    using Moq;

    using Xunit;

    public class SystemClientAssetsExtensionsTests
    {
        [Fact]
        public void TestAddServerSetup_ShouldAddExpectedServices(
        )
        {
            // Given
            var serviceCollectionMock = new ServiceCollectionMock();

            // When
            SystemClientAssetsExtensions.AddSystemClientAssets(
                serviceCollectionMock
            );

            // Then
            Assert.Collection(
                serviceCollectionMock,
                service =>
                {
                    service.ServiceType.Should().Be(typeof(ClientAssetRepository));
                    service.ImplementationType.Should().Be(typeof(ClientAssetInMemoryRepository));
                }
            );
        }

        [Theory, AutoMoqData]
        public void TestShouldReturnApplicationBuilderForChainingCommands(
            // Given
            [Frozen] Mock<IMediator> mediatorMock
        )
        {
            var applicationBuilderMocks = ApplicationBuilderFactory.CreateApplicationBuilder();
            var expected = new LoadSystemClientAssetsCommand();

            applicationBuilderMocks.ServiceProviderMock.Setup(
                mock => mock.GetService(typeof(IMediator))
            ).Returns(
                mediatorMock.Object
            );
            // When
            var actual = SystemClientAssetsExtensions.UseSystemClientAssets(
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
