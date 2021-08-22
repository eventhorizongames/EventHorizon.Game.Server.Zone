namespace EventHorizon.Zone.System.Watcher.Tests.State
{
    using EventHorizon.Zone.System.Watcher.State;

    using Xunit;

    public class InMemoryPendingReloadStateTests
    {
        [Fact]
        public void TestShouldDefaultIsPendingIsTruenWhenFirstCreated()
        {
            // Given

            // When
            var pendingReloadState = new InMemoryPendingReloadState();

            // Then
            Assert.False(
                pendingReloadState.IsPending
            );
        }

        [Fact]
        public void TestShouldSetIsPendingToTrueWhenSetToPendingIsCalled()
        {
            // Given

            // When
            var pendingReloadState = new InMemoryPendingReloadState();

            pendingReloadState.SetToPending();

            // Then
            Assert.True(
                pendingReloadState.IsPending
            );
        }

        [Fact]
        public void TestShouldSetIsPendingToFalseWhenSetToRemovePendingIsCalled()
        {
            // Given

            // When
            var pendingReloadState = new InMemoryPendingReloadState();

            pendingReloadState.SetToPending();

            // Make Sure IsPending is True
            Assert.True(
                pendingReloadState.IsPending
            );

            pendingReloadState.RemovePending();

            // Then
            Assert.False(
                pendingReloadState.IsPending
            );
        }
    }
}
