namespace EventHorizon.Zone.System.Wizard.Tests.Run
{
    using EventHorizon.Zone.System.Server.Scripts.Events.Run;
    using EventHorizon.Zone.System.Server.Scripts.Model;
    using EventHorizon.Zone.System.Wizard.Api;
    using EventHorizon.Zone.System.Wizard.Events.Run;
    using EventHorizon.Zone.System.Wizard.Model;
    using EventHorizon.Zone.System.Wizard.Model.Scripts;
    using EventHorizon.Zone.System.Wizard.Run;
    using FluentAssertions;
    using global::System.Collections.Generic;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;
    using Moq;
    using Xunit;

    public class RunWizardScriptProcessorCommandHandlerTests
    {

        [Fact]
        public async Task ShouldReturnScucessWhenScriptRunCommandIsSuccessful()
        {
            // Given
            var wizardId = "wizard-id";
            var wizardStepId = "wizard-step-id";
            var processorScriptId = "processor-script-id";
            var wizardData = new WizardData();

            var wizardStep = new WizardStep
            {
                Id = wizardStepId,
                Name = "wziard-step-name",
                Description = "wziard-step-description",
                Details = new WizardStepDetails
                {
                    ["Processor:ScriptId"] = processorScriptId,
                },
                NextStep = "next-step-id",
                PreviousStep = "previous-step-id",
            };
            var wizard = new WizardMetadata
            {
                Id = wizardId,
                StepList = new List<WizardStep>
                {
                    wizardStep,
                },
            };

            var actualScriptCommand = default(RunServerScriptCommand);

            var mediatorMock = new Mock<IMediator>();
            var wizardRepositoryMock = new Mock<WizardRepository>();

            var serverScriptResponse = new WizardServerScriptResponse(
                true,
                string.Empty
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    It.Is<RunServerScriptCommand>(
                        a => a.Id == processorScriptId
                    ),
                    CancellationToken.None
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
            var handler = new RunWizardScriptProcessorCommandHandler(
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

        [Fact]
        public async Task ShouldReturnWizardNotFoundErrorCodeWhenWizardIsNotInRepository()
        {
            // Given
            var wizardId = "wizard-id";
            var wizardStepId = "wizard-step-id";
            var processorScriptId = "processor-script-id";
            var wizardData = new WizardData();

            var expected = "wizard_not_found";

            var mediatorMock = new Mock<IMediator>();
            var wizardRepositoryMock = new Mock<WizardRepository>();

            // When
            var handler = new RunWizardScriptProcessorCommandHandler(
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

        [Fact]
        public async Task ShouldReturnWizardStepNotFoundErrorCodeWhenWizardDoesNotContainStep()
        {
            // Given
            var wizardId = "wizard-id";
            var wizardStepId = "wizard-step-id";
            var processorScriptId = "processor-script-id";
            var wizardData = new WizardData();

            var wizard = new WizardMetadata
            {
                Id = wizardId,
            };

            var expected = "wizard_step_not_found";

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

        [Fact]
        public async Task ShouldReturnFailedScriptResponseWhenScriptRunCommandIsFailure()
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
                            ["Processor:ScriptId"] = processorScriptId,
                        }
                    }
                }
            };
            var serverScriptResponse = new WizardServerScriptResponse(
                false,
                "failed_script_message"
            );

            var expected = "failed_script_message";

            var mediatorMock = new Mock<IMediator>();
            var wizardRepositoryMock = new Mock<WizardRepository>();

            mediatorMock.Setup(
                mock => mock.Send(
                    It.IsAny<RunServerScriptCommand>(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                serverScriptResponse
            );

            wizardRepositoryMock.Setup(
                mock => mock.Get(
                    wizardId

                )
            ).Returns(
                wizard.ToOption()
            );

            // When
            var handler = new RunWizardScriptProcessorCommandHandler(
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

        [Fact]
        public async Task ShouldReturnFailedScriptResponseWhenScriptRunCommandReturnNull()
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
                            ["Processor:ScriptId"] = processorScriptId,
                        }
                    }
                }
            };

            var expected = "wizard_failed_script_run";

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
