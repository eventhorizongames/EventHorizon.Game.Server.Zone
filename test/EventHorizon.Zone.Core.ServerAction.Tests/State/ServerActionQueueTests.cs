using Xunit;
using System;
using EventHorizon.Zone.Core.ServerAction.Model;
using EventHorizon.Tests.TestUtils;
using EventHorizon.Zone.Core.ServerAction.State;

namespace EventHorizon.Zone.Core.ServerAction.Tests.State
{
    public class ServerActionQueueTests
    {
        [Fact]
        public void TestShouldReturnAmountOfActionsThatAreReadyToBeProcessed()
        {
            // Given
            var input = 10;
            var expectedActionEntity1 = new ServerActionEntity(
                DateTime.UtcNow.Subtract(
                    TimeSpan.FromMinutes(1)
                ), 
                new TestNotificationEvent()
            );
            var expectedActionEntity2 = new ServerActionEntity(
                DateTime.UtcNow.Subtract(
                    TimeSpan.FromMinutes(23)
                ), 
                new TestNotificationEvent()
            );
            var expectedActionEntity3 = new ServerActionEntity(
                DateTime.UtcNow.Subtract(
                    TimeSpan.FromMinutes(44)
                ), 
                new TestNotificationEvent()
            );
            var actionEntity4 = new ServerActionEntity(
                DateTime.UtcNow.Add(
                    TimeSpan.FromMinutes(4)
                ), 
                new TestNotificationEvent()
            );
            var actionEntity5 = new ServerActionEntity(
                DateTime.UtcNow.Add(
                    TimeSpan.FromMinutes(3)
                ), 
                new TestNotificationEvent()
            );

            // When
            var actionRepository = new ServerActionQueue();
            actionRepository.Push(
                expectedActionEntity1
            );
            actionRepository.Push(
                expectedActionEntity2
            );
            actionRepository.Push(
                expectedActionEntity3
            );
            actionRepository.Push(
                actionEntity4
            );
            actionRepository.Push(
                actionEntity5
            );

            var actual = actionRepository.Take(
                input
            );

            // Then
            Assert.Collection(actual,
                a => Assert.Equal(expectedActionEntity1, a),
                a => Assert.Equal(expectedActionEntity2, a),
                a => Assert.Equal(expectedActionEntity3, a)
            );
        }

        [Fact]
        public void TestShouldRemoveReturnedEntitiesFromFutureTakes()
        {
            // Given
            var input = 10;
            var expectedActionEntity1 = new ServerActionEntity(
                DateTime.UtcNow.Subtract(
                    TimeSpan.FromMinutes(1)
                ), 
                new TestNotificationEvent()
            );
            var expectedActionEntity2 = new ServerActionEntity(
                DateTime.UtcNow.Subtract(
                    TimeSpan.FromMinutes(23)
                ), 
                new TestNotificationEvent()
            );
            var expectedActionEntity3 = new ServerActionEntity(
                DateTime.UtcNow.Subtract(
                    TimeSpan.FromMinutes(44)
                ), 
                new TestNotificationEvent()
            );
            var actionEntity4 = new ServerActionEntity(
                DateTime.UtcNow.Add(
                    TimeSpan.FromMinutes(4)
                ), 
                new TestNotificationEvent()
            );
            var actionEntity5 = new ServerActionEntity(
                DateTime.UtcNow.Add(
                    TimeSpan.FromMinutes(3)
                ), 
                new TestNotificationEvent()
            );

            // When
            var actionRepository = new ServerActionQueue();
            actionRepository.Push(
                expectedActionEntity1
            );
            actionRepository.Push(
                expectedActionEntity2
            );
            actionRepository.Push(
                expectedActionEntity3
            );
            actionRepository.Push(
                actionEntity4
            );
            actionRepository.Push(
                actionEntity5
            );

            var actualFirstCall = actionRepository.Take(
                input
            );
            var actualSecondCall = actionRepository.Take(
                input
            );

            // Then
            Assert.Collection(
                actualFirstCall,
                a => Assert.Equal(expectedActionEntity1, a),
                a => Assert.Equal(expectedActionEntity2, a),
                a => Assert.Equal(expectedActionEntity3, a)
            );

            Assert.Empty(
                actualSecondCall
            );
        }

        [Fact]
        public void TestShouldOnlyReturnTakeAmountWhenContainingMoreThanTake()
        {
            // Given
            var input = 2;
            var expectedActionEntity1 = new ServerActionEntity(
                DateTime.UtcNow.Subtract(
                    TimeSpan.FromMinutes(1)
                ), 
                new TestNotificationEvent()
            );
            var expectedActionEntity2 = new ServerActionEntity(
                DateTime.UtcNow.Subtract(
                    TimeSpan.FromMinutes(23)
                ), 
                new TestNotificationEvent()
            );
            var actionEntity3 = new ServerActionEntity(
                DateTime.UtcNow.Subtract(
                    TimeSpan.FromMinutes(44)
                ), 
                new TestNotificationEvent()
            );
            var actionEntity4 = new ServerActionEntity(
                DateTime.UtcNow.Subtract(
                    TimeSpan.FromMinutes(4)
                ), 
                new TestNotificationEvent()
            );
            var actionEntity5 = new ServerActionEntity(
                DateTime.UtcNow.Subtract(
                    TimeSpan.FromMinutes(3)
                ), 
                new TestNotificationEvent()
            );

            // When
            var actionRepository = new ServerActionQueue();
            actionRepository.Push(
                expectedActionEntity1
            );
            actionRepository.Push(
                expectedActionEntity2
            );
            actionRepository.Push(
                actionEntity3
            );
            actionRepository.Push(
                actionEntity4
            );
            actionRepository.Push(
                actionEntity5
            );

            var actual = actionRepository.Take(
                input
            );

            // Then
            Assert.Collection(
                actual,
                a => Assert.Equal(expectedActionEntity1, a),
                a => Assert.Equal(expectedActionEntity2, a)
            );
        }
    }
}