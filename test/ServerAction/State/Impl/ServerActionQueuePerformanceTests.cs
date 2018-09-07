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
    public class ServerActionQueuePerformanceTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public ServerActionQueuePerformanceTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            Prime();
        }
        private void Prime()
        {
            var input = 1_000;
            var entitiesToCreate = 1_000_000;
            var actionRepository = new ServerActionQueue();
            var now = DateTime.UtcNow;
            for (int i = 0; i < entitiesToCreate; i++)
            {
                actionRepository.Push(new ServerActionEntity(now.Subtract(TimeSpan.FromMinutes(1)))).GetAwaiter().GetResult();
            }
            actionRepository.Take(input).GetAwaiter().GetResult();
        }

        [Fact]
        [Trait("Category", "Performance")]
        public async Task TestTake_ShouldRemoveExpectedEntityList()
        {
            // Given
            var expectedLessThan = 15;
            var input = 1_000;
            var entitiesToCreate = 1_000_000;

            // When
            var actionRepository = new ServerActionQueue();
            var now = DateTime.UtcNow;
            for (int i = 0; i < entitiesToCreate; i++)
            {
                await actionRepository.Push(new ServerActionEntity(now.Subtract(TimeSpan.FromMinutes(1))));
            }

            var watch = Stopwatch.StartNew();
            var actual = await actionRepository.Take(input);

            // Then
            var elapsed = watch.ElapsedMilliseconds;
            _testOutputHelper.WriteLine("Time: {0}ms", elapsed);
            _testOutputHelper.WriteLine("Count: {0}", actual.Count());
            Assert.True(elapsed <= expectedLessThan);
        }

        [Fact]
        [Trait("Category", "Performance")]
        public async Task TestTake_ShouldBeWithInLimit_3()
        {
            // Given
            var expectedLessThan = 3;
            var input = 100;
            var entitiesToCreate = 100_000;

            // When
            var actionRepository = new ServerActionQueue();
            var now = DateTime.UtcNow;
            for (int i = 0; i < entitiesToCreate; i++)
            {
                await actionRepository.Push(new ServerActionEntity(now.Subtract(TimeSpan.FromMinutes(1))));
            }

            var watch = Stopwatch.StartNew();
            var actual = await actionRepository.Take(input);

            // Then
            var elapsed = watch.ElapsedMilliseconds;
            _testOutputHelper.WriteLine("Time: {0}ms", elapsed);
            _testOutputHelper.WriteLine("Count: {0}", actual.Count());
            Assert.True(elapsed <= expectedLessThan);
        }

        [Fact]
        [Trait("Category", "Performance")]
        public async Task TestTake_ShouldBeWithInLimit_2()
        {
            // Given
            var expectedLessThan = 2;
            var input = 10;
            var entitiesToCreate = 1_000;

            // When
            var actionRepository = new ServerActionQueue();
            var now = DateTime.UtcNow;
            for (int i = 0; i < entitiesToCreate; i++)
            {
                await actionRepository.Push(new ServerActionEntity(now.Subtract(TimeSpan.FromMinutes(1))));
            }

            var watch = Stopwatch.StartNew();
            var actual = await actionRepository.Take(input);

            // Then
            var elapsed = watch.ElapsedMilliseconds;
            _testOutputHelper.WriteLine("Time: {0}ms", elapsed);
            _testOutputHelper.WriteLine("Count: {0}", actual.Count());
            Assert.True(elapsed <= expectedLessThan);
        }

        [Fact]
        [Trait("Category", "Performance")]
        public async Task TestTake_ShouldBeWithInLimit_1()
        {
            // Given
            var expectedLessThan = 1;
            var input = 1;
            var entitiesToCreate = 1;

            // When
            var actionRepository = new ServerActionQueue();
            var now = DateTime.UtcNow;
            for (int i = 0; i < entitiesToCreate; i++)
            {
                await actionRepository.Push(new ServerActionEntity(now.Subtract(TimeSpan.FromMinutes(1))));
            }

            var watch = Stopwatch.StartNew();
            var actual = await actionRepository.Take(input);

            // Then
            var elapsed = watch.ElapsedMilliseconds;
            _testOutputHelper.WriteLine("Time: {0}ms", elapsed);
            _testOutputHelper.WriteLine("Count: {0}", actual.Count());
            Assert.True(elapsed <= expectedLessThan);
        }

        [Fact]
        [Trait("Category", "Performance")]
        public async Task TestTake_ShouldTakeAllBeWithInLimit_1()
        {
            // Given
            var expectedLessThan = 1;
            var input = 100;
            var entitiesToCreate = 100;

            // When
            var actionRepository = new ServerActionQueue();
            var now = DateTime.UtcNow;
            for (int i = 0; i < entitiesToCreate; i++)
            {
                await actionRepository.Push(new ServerActionEntity(now.Subtract(TimeSpan.FromMinutes(1))));
            }

            var watch = Stopwatch.StartNew();
            var actual = await actionRepository.Take(input);

            // Then
            _testOutputHelper.WriteLine("Time: {0}ms", watch.ElapsedMilliseconds);
            _testOutputHelper.WriteLine("Count: {0}", actual.Count());
            Assert.True(watch.ElapsedMilliseconds <= expectedLessThan);
        }
    }
}