using Xunit;
using Moq;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using EventHorizon.Zone.System.Agent.Save;
using System.Threading.Tasks;
using System.Threading;
using EventHorizon.Zone.System.Agent.Save.Events;

namespace EventHorizon.Game.Server.Zone.Tests.Agent.Save
{
    public class SaveAgentStateScheduledTaskTests
    {
        [Fact]
        public async Task TestExecuteAsync_ShouldPublishEventToSaveAgentStateEvent()
        {
            // Given
            var expectedSaveAgentStateEvent = new SaveAgentStateEvent();

            var mediatorMock = new Mock<IMediator>();
            var serviceScopeMock = new Mock<IServiceScope>();
            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(IMediator))).Returns(mediatorMock.Object);
            serviceScopeMock.SetupGet(serviceScope => serviceScope.ServiceProvider).Returns(serviceProviderMock.Object);
            var serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();
            serviceScopeFactoryMock.Setup(serviceScopeFactory => serviceScopeFactory.CreateScope()).Returns(serviceScopeMock.Object);

            // When
            var saveAgentStateScheduledTask = new SaveAgentStateScheduledTask(
                serviceScopeFactoryMock.Object
            );

            await saveAgentStateScheduledTask.ExecuteAsync(CancellationToken.None);

            // Then
            mediatorMock.Verify(mediator => mediator.Publish(expectedSaveAgentStateEvent, CancellationToken.None));
        }

        [Fact]
        public void TestSchedule_ShouldBeExpectedScheduleString()
        {
            // Given
            var expectedSchedule = "*/5 * * * * *"; // Every 5 Seconds

            // When
            var saveAgentStateScheduledTask = new SaveAgentStateScheduledTask(null);

            // Then
            Assert.Equal(expectedSchedule, saveAgentStateScheduledTask.Schedule);
        }
    }
}