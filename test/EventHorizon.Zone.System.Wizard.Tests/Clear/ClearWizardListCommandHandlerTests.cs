namespace EventHorizon.Zone.System.Wizard.Tests.Clear
{
    using EventHorizon.Zone.System.Wizard.Api;
    using EventHorizon.Zone.System.Wizard.Clear;
    using FluentAssertions;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using Moq;
    using Xunit;

    public class ClearWizardListCommandHandlerTests
    {
        [Fact]
        public async Task ShouldClearWizardRepositoryWhenCommandIsHandled()
        {
            // Given
            var wizardRepositoryMock = new Mock<WizardRepository>();

            // When
            var handler = new ClearWizardListCommandHandler(
                wizardRepositoryMock.Object
            );
            var actual = await handler.Handle(
                new ClearWizardListCommand(),
                CancellationToken.None
            );

            // Then
            actual.Success.Should().BeTrue();

            wizardRepositoryMock.Verify(
                mock => mock.Clear()
            );
        }
    }
}
