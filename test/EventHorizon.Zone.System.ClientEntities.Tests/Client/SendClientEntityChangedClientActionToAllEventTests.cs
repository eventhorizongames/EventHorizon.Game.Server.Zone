namespace EventHorizon.Zone.System.ClientEntities.Tests.Client
{
    using EventHorizon.Zone.System.ClientEntities.Client;
    using EventHorizon.Zone.System.ClientEntities.Model;
    using EventHorizon.Zone.System.ClientEntities.Model.Client;
    using FluentAssertions;
    using Xunit;

    public class SendClientEntityChangedClientActionToAllEventTests
    {
        [Fact]
        public void ShouldHaveExpectedActionWhenCreated()
        {
            // Given
            var expected = "SERVER_CLIENT_ENTITY_CHANGED_CLIENT_ACTION_EVENT";
            var expectedDetails = new ClientEntity();

            // When
            var actual = SendClientEntityChangedClientActionToAllEvent.Create(
                new ClientEntityChangedClientActionData(
                    expectedDetails
                )
            );

            // Then
            actual.Action
                .Should()
                .Be(
                    expected
                );
            actual.Data
                .As<ClientEntityChangedClientActionData>()
                .Details
                .Should()
                .Be(
                    expectedDetails
                );
        }
    }
}