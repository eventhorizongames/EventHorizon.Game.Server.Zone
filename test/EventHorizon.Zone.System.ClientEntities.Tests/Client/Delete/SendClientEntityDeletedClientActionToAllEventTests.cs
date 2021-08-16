namespace EventHorizon.Zone.System.ClientEntities.Tests.Client.Delete
{
    using EventHorizon.Zone.System.ClientEntities.Client.Delete;
    using EventHorizon.Zone.System.ClientEntities.Model.Client;

    using FluentAssertions;

    using Xunit;

    public class SendClientEntityDeletedClientActionToAllEventTests
    {
        [Fact]
        public void ShouldHaveExpectedActionWhenCreated()
        {
            // Given
            var expected = "SERVER_CLIENT_ENTITY_DELETED_CLIENT_ACTION_EVENT";
            var expectedGlobalId = "global-id";

            // When
            var actual = SendClientEntityDeletedClientActionToAllEvent.Create(
                new ClientEntityDeletedClientActionData(
                    expectedGlobalId
                )
            );

            // Then
            actual.Action
                .Should()
                .Be(
                    expected
                );
            actual.Data
                .As<ClientEntityDeletedClientActionData>()
                .GlobalId
                .Should()
                .Be(
                    expectedGlobalId
                );
        }
    }
}
