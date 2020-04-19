using System.Threading.Tasks;
using EventHorizon.Zone.System.ClientEntities.Client;
using EventHorizon.Zone.System.ClientEntities.Model;
using EventHorizon.Zone.System.ClientEntities.Model.Client;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;

namespace EventHorizon.Zone.System.ClientEntities.Tests.Client
{
    public class SendClientEntityChangedClientActionToAllEventTests
    {
        [Fact]
        public void ShouldHaveExpectedActionWhenCreated()
        {
            // Given
            var expected = "SERVER_CLIENT_ENTITY_CHANGED_CLIENT_ACTION_EVENT";
            var expectedDetails = new ClientEntity();

            // When
            var actual = new SendClientEntityChangedClientActionToAllEvent(
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
                .Details
                .Should()
                .Be(
                    expectedDetails
                );
        }

        [Fact]
        public void ShouldAcceptMediatorWhenHandlerIsCreated()
        {
            // Given
            var mediatorMock = new Mock<IMediator>();

            // When
            var actual = new SendClientEntityChangedClientActionToAllEventHandler(
                mediatorMock.Object
            );

            // Then
            actual.Should().NotBeNull();
        }
    }
}