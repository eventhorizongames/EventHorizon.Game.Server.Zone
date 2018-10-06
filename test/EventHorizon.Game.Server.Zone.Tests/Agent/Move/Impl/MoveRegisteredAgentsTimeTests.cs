using Xunit;
using Moq;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using System;
using EventHorizon.Game.Server.Zone.Agent.Move.Impl;
using Microsoft.Extensions.Logging;
using System.Threading;
using EventHorizon.Game.Server.Zone.Agent.Move;
using static EventHorizon.Game.Server.Zone.Agent.Move.Impl.MoveRegisteredAgentsTimer;

namespace EventHorizon.Game.Server.Zone.Tests.Agent.Move.Impl
{
    public class MoveRegisteredAgentsTimeTests
    {
        [Fact]
        public void TestStart_ShouldPublishMoveRegisteredAgentsEventAfterSetAmountOfTime()
        {
            // Given
            var expectedPeriod = 100;
            var expectedEvent = new MoveRegisteredAgentsEvent();

            // When
            var actual = new MoveRegisteredAgentsTimer();

            // Then
            Assert.Equal(expectedPeriod, actual.Period);
            Assert.Equal(expectedEvent, actual.OnRunEvent);
        }

    }
}