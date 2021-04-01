namespace EventHorizon.Zone.System.Wizard.Tests.Run
{
    using EventHorizon.Zone.System.Wizard.Events.Run;
    using EventHorizon.Zone.System.Wizard.Model;
    using EventHorizon.Zone.System.Wizard.Run;
    using FluentAssertions;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using Xunit;


    public class RunWizardScriptProcessorCommandHandlerTests
    {
        [Fact]
        public async Task ShouldReturnNotImplementedErrorCodeWhenCommandIsHandled()
        {
            // Given
            var wizardId = "wizard-id";
            var wizardStepId = "wizard-step-id";
            var processorScriptId = "processor-script-id";
            var wizardData = new WizardData();

            var expected = "NOT_IMPLEMENTED";

            // When
            var handler = new RunWizardScriptProcessorCommandHandler(

            );
            var actual = await handler.Handle(
                new RunWizardScriptProcessorCommand(
                    wizardId,
                    wizardStepId,
                    processorScriptId,
                    wizardData
                ),
                CancellationToken.None
            );

            // Then
            actual.Success
                .Should().BeFalse();
            actual.ErrorCode
                .Should().Be(expected);
        }
    }
}
