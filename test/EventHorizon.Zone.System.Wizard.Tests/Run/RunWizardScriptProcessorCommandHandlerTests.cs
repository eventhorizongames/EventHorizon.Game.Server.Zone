namespace EventHorizon.Zone.System.Wizard.Tests.Run
{
    using AutoFixture.Xunit2;

    using EventHorizon.Test.Common.Attributes;
    using EventHorizon.Zone.System.Server.Scripts.Events.Run;
    using EventHorizon.Zone.System.Server.Scripts.Model;
    using EventHorizon.Zone.System.Wizard.Api;
    using EventHorizon.Zone.System.Wizard.Events.Run;
    using EventHorizon.Zone.System.Wizard.Model;
    using EventHorizon.Zone.System.Wizard.Model.Scripts;
    using EventHorizon.Zone.System.Wizard.Run;

    using FluentAssertions;

    using global::System;
    using global::System.Collections.Generic;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    using Microsoft.Extensions.Logging;

    using Moq;

    using Xunit;

    public class RunWizardScriptProcessorCommandHandlerTests
    {
        [Theory, AutoMoqData]
        public async Task ShouldReturnScucessWhenScriptRunCommandIsSuccessful(
            // Given
            [Frozen] Mock<IMediator> mediatorMock,
            [Frozen] Mock<WizardRepository> wizardRepositoryMock,
            string wizardId,
            string wizardStepId,
            string processorScriptId,
            [Frozen] WizardData wizardData,
            [Frozen] WizardStep wizardStep,
            [Frozen] WizardMetadata wizard,
            RunWizardScriptProcessorCommandHandler handler
        )
        {
            wizardStep.Id = wizardStepId;
            wizardStep.Details = new WizardStepDetails
            {
                ["Processor:ScriptId"] = processorScriptId,
            };

            wizard.Id = wizardId;
            wizard.StepList = new List<WizardStep>
            {
                wizardStep,
            };

            var actualScriptCommand = default(RunServerScriptCommand);

            var serverScriptResponse = new WizardServerScriptResponse(
                true,
                string.Empty
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    It.Is<RunServerScriptCommand>(
                        a => a.Id == processorScriptId
                    ),
                    It.IsAny<CancellationToken>()
                )
            ).ReturnsAsync(
                serverScriptResponse
            ).Callback<IRequest<ServerScriptResponse>, CancellationToken>(
                (request, _) =>
                {
                    if (request is RunServerScriptCommand command)
                    {
                        actualScriptCommand = command;
                    }
                }
            );

            wizardRepositoryMock.Setup(
                mock => mock.Get(
                    wizardId
                )
            ).Returns(
                wizard.ToOption()
            );

            // When
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
                .Should().BeTrue();

            actualScriptCommand.Id
                .Should().Be(processorScriptId);
            actualScriptCommand.Data
                .Should().BeEquivalentTo(
                    new Dictionary<string, object>
                    {
                        ["Wizard"] = wizard,
                        ["WizardStep"] = wizardStep,
                        ["WizardData"] = wizardData,
                    }
                );
        }

        [Theory, AutoMoqData]
        public async Task ShouldReturnWizardNotFoundErrorCodeWhenWizardIsNotInRepository(
            // Given
            [Frozen] Mock<WizardRepository> wizardRepositoryMock,
            string wizardId,
            string wizardStepId,
            string processorScriptId,
            WizardData wizardData,
            RunWizardScriptProcessorCommandHandler handler
        )
        {
            var expected = "wizard_not_found";

            wizardRepositoryMock.Setup(
                mock => mock.Get(
                    It.IsAny<string>()
                )
            ).Returns(
                default(WizardMetadata)
            );

            // When
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

        [Theory, AutoMoqData]
        public async Task ShouldReturnWizardStepNotFoundErrorCodeWhenWizardDoesNotContainStep(
            // Given
            string wizardId,
            string wizardStepId,
            string processorScriptId,
            WizardData wizardData,
            RunWizardScriptProcessorCommandHandler handler
        )
        {
            var expected = "wizard_step_not_found";

            // When
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

        [Theory, AutoMoqData]
        public async Task ShouldReturnFailedScriptResponseWhenScriptRunCommandIsFailure(
            // Given
            [Frozen] Mock<IMediator> mediatorMock,
            string wizardId,
            string wizardStepId,
            string processorScriptId,
            [Frozen] WizardStepDetails wizardStepDetails,
            [Frozen] WizardStep wizardStep,
            [Frozen] WizardData wizardData,
            [Frozen] WizardMetadata wizard,
            RunWizardScriptProcessorCommandHandler handler
        )
        {
            // Given
            wizard.Id = wizardId;
            wizardStep.Id = wizardStepId;
            wizardStepDetails["Processor:ScriptId"] = processorScriptId;

            var serverScriptResponse = new WizardServerScriptResponse(
                false,
                "failed_script_message"
            );

            var expected = "failed_script_message";

            mediatorMock.Setup(
                mock => mock.Send(
                    It.IsAny<RunServerScriptCommand>(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                serverScriptResponse
            );

            // When
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

        [Theory, AutoMoqData]
        public async Task ShouldReturnFailedScriptResponseWhenScriptRunCommandReturnNull(
            // Given
            string wizardId,
            string wizardStepId,
            string processorScriptId,
            [Frozen] WizardStepDetails wizardStepDetails,
            [Frozen] WizardStep wizardStep,
            [Frozen] WizardData wizardData,
            [Frozen] WizardMetadata wizard,
            RunWizardScriptProcessorCommandHandler handler
        )
        {
            wizard.Id = wizardId;
            wizardStep.Id = wizardStepId;
            wizardStepDetails["Processor:ScriptId"] = processorScriptId;

            var expected = "wizard_failed_script_run";

            // When
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

        [Theory, AutoMoqData]
        public async Task ShouldReturnFailedScriptResponseWhenScriptRunCommandReturnIsNotWizardServerScriptResponse(
            // Given
            [Frozen] Mock<IMediator> mediatorMock,
            string wizardId,
            string wizardStepId,
            string processorScriptId,
            [Frozen] WizardStepDetails wizardStepDetails,
            [Frozen] WizardStep wizardStep,
            [Frozen] WizardData wizardData,
            [Frozen] WizardMetadata wizard,
            RunWizardScriptProcessorCommandHandler handler
        )
        {
            wizard.Id = wizardId;
            wizardStep.Id = wizardStepId;
            wizardStepDetails["Processor:ScriptId"] = processorScriptId;

            var expected = "wizard_failed_script_run";

            var serverScriptResponseMock = new Mock<ServerScriptResponse>();
            serverScriptResponseMock.Setup(
                mock => mock.Success
            ).Returns(true);

            mediatorMock.Setup(
                mock => mock.Send(
                    It.IsAny<RunServerScriptCommand>(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                serverScriptResponseMock.Object
            );

            // When
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

        [Theory, AutoMoqData]
        public async Task ShouldReturnFailedScriptRunWhenScriptRunCommandThrowsException(
            // Given
            [Frozen] Mock<IMediator> mediatorMock,
            string wizardId,
            string wizardStepId,
            string processorScriptId,
            [Frozen] WizardStepDetails wizardStepDetails,
            [Frozen] WizardStep wizardStep,
            [Frozen] WizardData wizardData,
            [Frozen] WizardMetadata wizard,
            RunWizardScriptProcessorCommandHandler handler
        )
        {
            wizard.Id = wizardId;
            wizardStep.Id = wizardStepId;
            wizardStepDetails["Processor:ScriptId"] = processorScriptId;

            var expected = "wizard_failed_script_run";

            mediatorMock.Setup(
                mock => mock.Send(
                    It.IsAny<RunServerScriptCommand>(),
                    CancellationToken.None
                )
            ).Throws(
                new Exception()
            );

            // When
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

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("               ")]
        [InlineData("invalid")]
        public async Task ShouldReturnInvalidProcessorScriptIdWhenProcessorScriptIdDoesNotMatchInStep(
            string stepProcessorScriptId
        )
        {
            // Given
            var wizardId = "wizard-id";
            var wizardStepId = "wizard-step-id";
            var processorScriptId = "processor-script-id";
            var wizardData = new WizardData();

            var wizard = new WizardMetadata
            {
                Id = wizardId,
                StepList = new List<WizardStep>
                {
                    new WizardStep
                    {
                        Id = wizardStepId,
                        Details = new WizardStepDetails
                        {
                            ["Processor:ScriptId"] = stepProcessorScriptId,
                        }
                    }
                }
            };

            var expected = "wizard_invalid_processor_script_id";

            var mediatorMock = new Mock<IMediator>();
            var wizardRepositoryMock = new Mock<WizardRepository>();

            wizardRepositoryMock.Setup(
                mock => mock.Get(
                    wizardId

                )
                ).Returns(
                    wizard.ToOption()
                );

            // When
            var handler = new RunWizardScriptProcessorCommandHandler(
                new Mock<ILogger<RunWizardScriptProcessorCommandHandler>>().Object,
                mediatorMock.Object,
                wizardRepositoryMock.Object
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
