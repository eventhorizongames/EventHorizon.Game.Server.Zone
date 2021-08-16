namespace EventHorizon.Zone.System.Wizard.Tests.Json.Merge
{
    using EventHorizon.Zone.Core.Model.Command;
    using EventHorizon.Zone.System.Wizard.Events.Json.Merge;
    using EventHorizon.Zone.System.Wizard.Json.Mapper;
    using EventHorizon.Zone.System.Wizard.Json.Merge;
    using EventHorizon.Zone.System.Wizard.Json.Model;
    using EventHorizon.Zone.System.Wizard.Model;

    using FluentAssertions;

    using global::System.Threading;
    using global::System.Threading.Tasks;

    using MediatR;

    using Moq;

    using Xunit;


    public class MergeWizardDataIntoJsonCommandHandlerTests
    {
        [Fact]
        public async Task ShouldReturnErrorCodeWhenMergeJsonStringFails()
        {
            // Given
            var sourceJson = "{}";
            var wizardData = new WizardData();
            var wizardJsonDocumentMock = new JsonDataDocument();
            var wizardJson = "{}";
            var expected = "error_from_merge_json_strings";

            var mediatorMock = new Mock<IMediator>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new MapWizardDataToJsonDataDocument(
                        wizardData
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                wizardJsonDocumentMock
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new MergeJsonStringsIntoSingleJsonStringCommand(
                        sourceJson,
                        wizardJson
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new CommandResult<string>(
                    expected
                )
            );

            // When
            var handler = new MergeWizardDataIntoJsonCommandHandler(
                mediatorMock.Object
            );
            var actual = await handler.Handle(
                new MergeWizardDataIntoJsonCommand(
                    wizardData,
                    sourceJson
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
        public async Task ShouldReturnResultFromMergeJsonStringWhenSuccessful()
        {
            // Given
            var sourceJson = "{}";
            var wizardData = new WizardData();
            var wizardJsonDocumentMock = new JsonDataDocument();
            var wizardJson = "{}";
            var expected = "{ \"merged-json\": \"value\" }";

            var mediatorMock = new Mock<IMediator>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new MapWizardDataToJsonDataDocument(
                        wizardData
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                wizardJsonDocumentMock
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    new MergeJsonStringsIntoSingleJsonStringCommand(
                        sourceJson,
                        wizardJson
                    ),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                new CommandResult<string>(
                    true,
                    expected
                )
            );

            // When
            var handler = new MergeWizardDataIntoJsonCommandHandler(
                mediatorMock.Object
            );
            var actual = await handler.Handle(
                new MergeWizardDataIntoJsonCommand(
                    wizardData,
                    sourceJson
                ),
                CancellationToken.None
            );

            // Then
            actual.Success
                .Should().BeTrue();
            actual.Result
                .Should().Be(expected);
        }
    }
}
