namespace EventHorizon.Zone.Core.ServerAction.Tests.State
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

    using EventHorizon.Tests.TestUtils;
    using EventHorizon.Zone.Core.ServerAction.Model;
    using EventHorizon.Zone.Core.ServerAction.State;

    using Xunit;
    using Xunit.Abstractions;

    public class ServerActionQueuePerformanceTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public ServerActionQueuePerformanceTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            Prime();
        }

        private static void Prime()
        {
            var input = 1_000;
            var entitiesToCreate = 1_000_000;
            var actionRepository = new ServerActionQueue();
            var now = DateTime.UtcNow;
            for (int i = 0; i < entitiesToCreate; i++)
            {
                actionRepository.Push(
                    new ServerActionEntity(
                        now.Subtract(
                            TimeSpan.FromMinutes(1)
                        ),
                        new TestNotificationEvent()
                    )
                );
            }
            actionRepository.Take(input);
        }

        [Theory]
        [Trait("Category", "Performance")]
        [InlineData(100, 1_000, 100_000)]
        [InlineData(5, 100, 10_000)]
        [InlineData(2, 10, 1_000)]
        [InlineData(1, 1, 1)]
        [InlineData(1, 100, 100)]
        public void TestShouldBeWithInLimitOfPassedInLimits(
            int expectedLessThan,
            int input,
            int entitiesToCreate
        )
        {
            // Given
            // When
            var actionRepository = new ServerActionQueue();
            var now = DateTime.UtcNow;
            for (int i = 0; i < entitiesToCreate; i++)
            {
                actionRepository.Push(
                    new ServerActionEntity(
                        now.Subtract(
                            TimeSpan.FromMinutes(1)
                        ),
                        new TestNotificationEvent()
                    )
                );
            }

            var watch = Stopwatch.StartNew();
            var actual = actionRepository.Take(
                input
            );

            // Then
            var elapsed = watch.ElapsedMilliseconds;
            _testOutputHelper.WriteLine(
                "{0} | {1} | {2}",
                expectedLessThan,
                input,
                entitiesToCreate
            );
            _testOutputHelper.WriteLine(
                "Time: {0}ms",
                elapsed
            );
            _testOutputHelper.WriteLine(
                "Count: {0}",
                actual.Count()
            );
            Assert.True(
                elapsed <= expectedLessThan
            );
            Assert.Equal(
                input,
                actual.Count()
            );
        }
    }
}
