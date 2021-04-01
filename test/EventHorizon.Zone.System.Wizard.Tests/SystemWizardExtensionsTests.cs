namespace EventHorizon.Zone.System.Wizard.Tests
{
    using EventHorizon.Game.Server.Zone;
    using EventHorizon.Test.Common;
    using EventHorizon.Test.Common.Utils;
    using EventHorizon.Zone.System.Wizard.Api;
    using EventHorizon.Zone.System.Wizard.Load;
    using EventHorizon.Zone.System.Wizard.State;
    using global::System.Threading;
    using MediatR;
    using Moq;
    using Xunit;

    public class SystemWizardExtensionsTests
    {
        [Fact]
        public void TestAddServerSetup_ShouldAddExpectedServices()
        {
            // Given
            var serviceCollectionMock = new ServiceCollectionMock();

            // When
            SystemWizardExtensions.AddSystemWizard(
                serviceCollectionMock
            );

            // Then
            Assert.Collection(
                serviceCollectionMock,
                service =>
                {
                    Assert.Equal(typeof(WizardRepository), service.ServiceType);
                    Assert.Equal(typeof(StandardWizardRepository), service.ImplementationType);
                }
            );
        }

        [Fact]
        public void TestShouldReturnApplicationBuilderForChainingCommands()
        {
            // Given
            var applicationBuilderMocks = ApplicationBuilderFactory.CreateApplicationBuilder();
            var expected = applicationBuilderMocks.ApplicationBuilderMock.Object;

            var mediatorMock = new Mock<IMediator>();

            applicationBuilderMocks.ServiceProviderMock.Setup(
                mock => mock.GetService(typeof(IMediator))
            ).Returns(
                mediatorMock.Object
            );

            // When
            var actual = SystemWizardExtensions.UseSystemWizard(
                applicationBuilderMocks.ApplicationBuilderMock.Object
            );

            // Then
            Assert.Equal(
                expected,
                actual
            );

            mediatorMock.Verify(
                mock => mock.Send(
                    new LoadSystemsWizardListCommand() as object,
                    It.IsAny<CancellationToken>()
                )
            );
        }
    }
}
