using Xunit;
using Moq;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using EventHorizon.Game.Server.Zone.Agent;
using System.Threading.Tasks;
using System;
using MediatR;
using EventHorizon.Game.Server.Zone.Agent.Model;
using System.Threading;
using System.Collections.Generic;
using EventHorizon.Game.Server.Zone.Agent.Get;

namespace EventHorizon.Game.Server.Zone.Tests.Agent
{
    public class AgentHubTests
    {
        [Fact]
        public async Task TestOnConnectedAsync_ShouldNotThrowExceptionWhenUserIsInAdminRole()
        {
            // Given
            var inputAdminRole = "Admin";

            var contextMock = new Mock<HubCallerContext>();
            var userMock = new Mock<ClaimsPrincipal>();
            contextMock.Setup(a => a.User).Returns(userMock.Object);
            userMock.Setup(a => a.IsInRole(inputAdminRole)).Returns(true);

            // When
            var agentHub = new AgentHub(null);
            agentHub.Context = contextMock.Object;

            await agentHub.OnConnectedAsync();

            // Then
            Assert.True(true);
        }
        [Fact]
        public async Task TestOnConnectedAsync_ShouldThrowNoRoleExceptionWhenUserIsNotInAdminRole()
        {
            // Given
            var expectedMessage = "no_role";
            var inputAdminRole = "Admin";

            var contextMock = new Mock<HubCallerContext>();
            var userMock = new Mock<ClaimsPrincipal>();
            contextMock.Setup(a => a.User).Returns(userMock.Object);
            userMock.Setup(a => a.IsInRole(inputAdminRole)).Returns(false);

            // When
            var agentHub = new AgentHub(null);
            agentHub.Context = contextMock.Object;
            try
            {
                await agentHub.OnConnectedAsync();
                Assert.False(true, "Should have threw a no_role exception.");
            }
            catch (Exception ex)
            {
                Assert.Equal(expectedMessage, ex.Message);
            }
        }
        [Fact]
        public async Task TestGetAgentList_ShouldReturnAgentEntityListFromGetAgentListEvent()
        {
            // Given
            var expectedAgent1 = new AgentEntity();
            var expectedAgent2 = new AgentEntity();
            var expectedAgent3 = new AgentEntity();
            var expectedAgentList = new List<AgentEntity>()
            {
                expectedAgent1,
                expectedAgent2,
                expectedAgent3
            };
            var expectedGetAgentListEvent = new GetAgentListEvent();

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(a => a.Send(expectedGetAgentListEvent, CancellationToken.None)).ReturnsAsync(expectedAgentList);

            // When
            var agentHub = new AgentHub(mediatorMock.Object);
            var actual = await agentHub.GetAgentList();

            mediatorMock.Verify(a => a.Send(expectedGetAgentListEvent, CancellationToken.None));
            Assert.Collection(actual,
                agent => Assert.Equal(expectedAgent1, agent),
                agent => Assert.Equal(expectedAgent2, agent),
                agent => Assert.Equal(expectedAgent3, agent)
            );
        }
    }
}