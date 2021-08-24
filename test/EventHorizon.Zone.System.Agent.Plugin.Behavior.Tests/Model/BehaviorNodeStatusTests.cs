namespace EventHorizon.Zone.System.Agent.Plugin.Behavior.Tests.Model
{
    using AutoFixture.Xunit2;

    using EventHorizon.Zone.System.Agent.Plugin.Behavior.Model;

    using FluentAssertions;

    using global::System.Collections;
    using global::System.Collections.Generic;

    using Xunit;

    public class BehaviorNodeStatusTests
    {
        [Theory]
        [ClassData(typeof(ShouldEqualStatusTestDataGenerator))]
        public void ShouldBeEqualWhenObjectMatchesObjectSingleSupportedValue(
            // Given
            BehaviorNodeStatus toEqual,
            BehaviorNodeStatus status
        )
        {
            // When
            var actual = status.Equals(
                toEqual
            );

            // Then
            actual.Should().BeTrue();
        }

        [Theory]
        [ClassData(typeof(ShouldEqualStringTestDataGenerator))]
        public void ShouldBeEqualWhenStringMatchesSingleSupportedValue(
            // Given
            string toEqual,
            BehaviorNodeStatus status
        )
        {
            // When
            var actual = status.Equals(
                toEqual
            );

            // Then
            actual.Should().BeTrue();
        }


        [Theory, AutoData]
        public void ShouldReturnFalseForEqualsCheckWhenNotValidType(
            // Given
            int toEqual
        )
        {
            var status = BehaviorNodeStatus.READY;

            // When
            var actual = status.Equals(
                toEqual
            );

            // Then
            actual.Should().BeFalse();
        }

        [Fact]
        public void ShouldReturnFalseWhenTryingToEqualsNotRelatedObject()
        {
            // Given
            // When
            var actual = BehaviorNodeStatus.ERROR.Equals(
                1000L
            );

            // Then
            actual.Should().BeFalse();
        }

        [Fact]
        public void GetHashCodeShouldNotReturnZeroWhenCalled()
        {
            // Given
            // When
            var actual = BehaviorNodeStatus.ERROR.GetHashCode();

            // Then
            actual.Should().NotBe(0);
        }


        public class ShouldEqualStatusTestDataGenerator
            : IEnumerable<object[]>
        {
            private readonly List<object[]> _data = new()
            {
                new object[] {
                    BehaviorNodeStatus.READY,
                    BehaviorNodeStatus.READY,
                },
                new object[] {
                    BehaviorNodeStatus.VISITING,
                    BehaviorNodeStatus.VISITING,
                },
                new object[] {
                    BehaviorNodeStatus.FAILED,
                    BehaviorNodeStatus.FAILED,
                },
                new object[] {
                    BehaviorNodeStatus.RUNNING,
                    BehaviorNodeStatus.RUNNING,
                },
                new object[] {
                    BehaviorNodeStatus.SUCCESS,
                    BehaviorNodeStatus.SUCCESS,
                },
                new object[] {
                    BehaviorNodeStatus.ERROR,
                    BehaviorNodeStatus.ERROR,
                },
            };

            public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        public class ShouldEqualStringTestDataGenerator
            : IEnumerable<object[]>
        {
            private readonly List<object[]> _data = new()
            {
                new object[] {
                    "READY",
                    BehaviorNodeStatus.READY
                },
                new object[] {
                    null,
                    BehaviorNodeStatus.READY
                },
                new object[] {
                    "VISITING",
                    BehaviorNodeStatus.VISITING
                },
                new object[] {
                    "FAILED",
                    BehaviorNodeStatus.FAILED
                },
                new object[] {
                    "RUNNING",
                    BehaviorNodeStatus.RUNNING
                },
                new object[] {
                    "SUCCESS",
                    BehaviorNodeStatus.SUCCESS
                },
                new object[] {
                    "ERROR",
                    BehaviorNodeStatus.ERROR
                },
            };

            public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
