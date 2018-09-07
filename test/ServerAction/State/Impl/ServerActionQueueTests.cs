using Xunit;
using Moq;
using EventHorizon.Game.Server.Zone.ServerAction.State.Impl;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.ServerAction.Model;
using System;
using Xunit.Abstractions;
using System.Diagnostics;
using System.Linq;

namespace EventHorizon.Game.Server.Zone.Tests.ServerAction.State.Impl
{
    public class ServerActionQueueTests
    {
        [Fact]
        public async Task TestClear_ShouldClearOutAnyEntityAdded()
        {
            // Given
            var input = 1;
            var expectedActionEntity1 = new ServerActionEntity(DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(1)));
            var expectedActionEntity2 = new ServerActionEntity(DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(23)));
            var expectedActionEntity3 = new ServerActionEntity(DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(44)));
            var actionEntity4 = new ServerActionEntity(DateTime.UtcNow.Add(TimeSpan.FromMinutes(4)));
            var actionEntity5 = new ServerActionEntity(DateTime.UtcNow.Add(TimeSpan.FromMinutes(3)));

            // When
            var actionRepository = new ServerActionQueue();
            await actionRepository.Push(expectedActionEntity1);
            await actionRepository.Push(expectedActionEntity2);
            await actionRepository.Push(expectedActionEntity3);
            await actionRepository.Push(actionEntity4);
            await actionRepository.Push(actionEntity5);
            await actionRepository.Clear();

            var actual = await actionRepository.Take(input);

            // Then
            Assert.Empty(actual);
        }

        [Fact]
        public async Task TestTake_ShouldReturnAmountOfActionsThatAreReadyToBeProcessed()
        {
            // Given
            var input = 10;
            var expectedActionEntity1 = new ServerActionEntity(DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(1)));
            var expectedActionEntity2 = new ServerActionEntity(DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(23)));
            var expectedActionEntity3 = new ServerActionEntity(DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(44)));
            var actionEntity4 = new ServerActionEntity(DateTime.UtcNow.Add(TimeSpan.FromMinutes(4)));
            var actionEntity5 = new ServerActionEntity(DateTime.UtcNow.Add(TimeSpan.FromMinutes(3)));

            // When
            var actionRepository = new ServerActionQueue();
            await actionRepository.Push(expectedActionEntity1);
            await actionRepository.Push(expectedActionEntity2);
            await actionRepository.Push(expectedActionEntity3);
            await actionRepository.Push(actionEntity4);
            await actionRepository.Push(actionEntity5);

            var actual = await actionRepository.Take(input);

            // Then
            Assert.Collection(actual,
                a => Assert.Equal(expectedActionEntity3, a),
                a => Assert.Equal(expectedActionEntity2, a),
                a => Assert.Equal(expectedActionEntity1, a)
            );
        }
    }
}