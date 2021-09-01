namespace EventHorizon.Zone.System.Server.Scripts.Plugin.BackgroundTask.Tests.Builders
{
    using EventHorizon.Test.Common.Attributes;
    using EventHorizon.Zone.System.Server.Scripts.Plugin.BackgroundTask.Builders;
    using EventHorizon.Zone.System.Server.Scripts.Plugin.BackgroundTask.Model;
    using EventHorizon.Zone.System.Server.Scripts.Plugin.BackgroundTask.Wrapper;

    using FluentAssertions;

    using Xunit;

    public class ThreadedBackgroundTaskWrapperBuilderTests
    {
        [Theory, AutoMoqData]
        public void ShouldReturnThreadedBackgroundTaskWrapperWhenBuildIsCalled(
            // Given
            ScriptedBackgroundTask task,
            ThreadedBackgroundTaskWrapperBuilder builder
        )
        {
            // When
            var actual = builder.Build(
                task
            );

            // Then
            actual.Should().BeOfType<ThreadedBackgroundTaskWrapper>();
        }
    }
}
