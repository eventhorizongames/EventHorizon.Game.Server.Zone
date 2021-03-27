namespace EventHorizon.Zone.System.Wizard.Tests.Query
{
    using EventHorizon.Zone.System.Wizard.Api;
    using EventHorizon.Zone.System.Wizard.Events.Query;
    using EventHorizon.Zone.System.Wizard.Model;
    using EventHorizon.Zone.System.Wizard.Query;
    using FluentAssertions;
    using global::System.Collections.Generic;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using Moq;
    using Xunit;

    public class QueryForAllWizardsHandlerTests
    {
        [Fact]
        public async Task ShouldReturnResultOfAllWizardsWhenRequestIsHandled()
        {
            // Given
            var wizardList = new List<WizardMetadata>
            {
                new WizardMetadata
                {
                    Id = "wizard-id",
                },
            };

            var expected = wizardList;

            var wizardRepositoryMock = new Mock<WizardRepository>();

            wizardRepositoryMock.Setup(
                mock => mock.All
            ).Returns(
                wizardList
            );

            // When
            var handler = new QueryForAllWizardsHandler(
                wizardRepositoryMock.Object
            );
            var actual = await handler.Handle(
                new QueryForAllWizards(),
                CancellationToken.None
            );

            // Then
            actual.Success.Should().BeTrue();
            actual.Result
                .Should().BeEquivalentTo(expected);
        }
    }
}
