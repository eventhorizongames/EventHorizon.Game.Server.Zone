using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace EventHorizon.TimerService.Tests
{
    public class HostedServiceTests
    {
        [Fact]
        public void TestShouldExecuteAsyncWhenStartAsync()
        {
            // Given
            var expected = Task.CompletedTask;

            // When
            var hostedService = new TestHostedService(
                expected
            );

            var actual = hostedService.StartAsync(
                CancellationToken.None
            );

            // Then
            Assert.Equal(
                expected,
                actual
            );
        }

        [Fact]
        public void TestShouldReturnCompletedTaskWhenExecutingTaskIsNotCompleted()
        {
            // Given
            var expected = Task.CompletedTask;

            var executingTask = new Task(() => Task.Delay(1000));

            // When
            var hostedService = new TestHostedService(
                executingTask
            );

            var actual = hostedService.StartAsync(
                CancellationToken.None
            );

            // Then
            Assert.Equal(
                expected,
                actual
            );
        }

        [Fact]
        public async Task TestShouldReturnWithoutErrorWhenExecutingTaskIsNull()
        {
            // Given
            Task responseOnExecute = null;

            // When
            var hostedService = new TestHostedService(
                responseOnExecute
            );

            await hostedService.StopAsync(
                CancellationToken.None
            );
            
            // Then
            Assert.True(true);
        }

        public class TestHostedService : HostedService
        {
            readonly Task _responseOnExecute;

            public TestHostedService(
                Task responseOnExecute
            )
            {
                _responseOnExecute = responseOnExecute;
            }

            protected override Task ExecuteAsync(
                CancellationToken cancellationToken
            )
            {
                return _responseOnExecute;
            }
        }
    }
}