namespace EventHorizon.Zone.Core.Map.Tests.Search.Collections
{
    using System;
    using System.Collections.Generic;

    using EventHorizon.Zone.Core.Map.Find.Search.Collections;

    using FluentAssertions;

    using Xunit;

    public class BinaryHeapTests
    {
        [Fact]
        public void ShouldCorrectlyOrderTheRawDataBasedOnComparePassedToConstructor()
        {
            // Given
            var input = new List<string>
            {
                "g",
                "k",
                "f",
                "h",
                "i",
                "j",
                "e",
                "d",
                "c",
                "b",
                "a",
            };
            var expectedOrder = new List<string>
            {
                "a",
                "b",
                "c",
                "d",
                "e",
                "f",
                "g",
                "h",
                "i",
                "j",
                "k",
            };
            var stringComparer = StringComparer.InvariantCulture;

            // When
            var binaryHeap = new BinaryHeap<string>(
                stringComparer
            );

            foreach (var stringValue in input)
            {
                binaryHeap.Add(
                    stringValue
                );
            }

            // Then
            foreach (var expected in expectedOrder)
            {
                var actual = binaryHeap.Dequeue();

                actual.Should().Be(expected);
            }
        }

        [Fact]
        public void ShouldReturnIsEmptyTrueWhenHasNoItemsInHeap()
        {
            // Given
            var stringComparer = StringComparer.InvariantCulture;

            // When
            var binaryHeap = new BinaryHeap<string>(
                stringComparer
            );
            var actual = binaryHeap.IsEmpty;

            // Then

            actual.Should().BeTrue();
        }

        [Fact]
        public void ShouldReturnIsEmptyFalseWhenHasAnyItemsInHeap()
        {
            // Given
            var input = "a";
            var stringComparer = StringComparer.InvariantCulture;

            // When
            var binaryHeap = new BinaryHeap<string>(
                stringComparer
            );
            binaryHeap.Add(
                input
            );
            var actual = binaryHeap.IsEmpty;

            // Then

            actual.Should().BeFalse();
        }

        [Fact]
        public void ShouldThrownInvalidOperationExceptionWhenQueueIsEmptyOnDequeue()
        {
            // Given
            var expected = "Priority queue is empty";
            var stringComparer = StringComparer.InvariantCulture;

            // When
            var binaryHeap = new BinaryHeap<string>(
                stringComparer
            );
            Action action = () => binaryHeap.Dequeue();
            var actual = Assert.Throws<InvalidOperationException>(
                action
            );

            // Then
            actual.Message
                .Should().Be(expected);
        }

        [Fact]
        public void ShouldThrownInvalidOperationExceptionWhenQueueIsEmptyOnPeek()
        {
            // Given
            var expected = "Priority queue is empty";
            var stringComparer = StringComparer.InvariantCulture;

            // When
            var binaryHeap = new BinaryHeap<string>(
                stringComparer
            );
            Action action = () => binaryHeap.Peek();
            var actual = Assert.Throws<InvalidOperationException>(
                action
            );

            // Then
            actual.Message
                .Should().Be(expected);
        }

        [Fact]
        public void ShouldEmptyHeapWhenClearIsCalled()
        {
            // Given
            var stringComparer = StringComparer.InvariantCulture;
            var input = "a";

            // When
            var binaryHeap = new BinaryHeap<string>(
                stringComparer
            );
            binaryHeap.Add(input);
            binaryHeap.IsEmpty
                .Should().BeFalse();
            binaryHeap.Clear();

            // Then
            binaryHeap.IsEmpty
                .Should().BeTrue();
        }

        [Fact]
        public void ShouldReturnTrueWhenValueIsInHeap()
        {
            // Given
            var stringComparer = StringComparer.InvariantCulture;
            var input = "a";

            // When
            var binaryHeap = new BinaryHeap<string>(
                stringComparer
            );
            binaryHeap.Add(input);

            // Then
            binaryHeap.Contains(input)
                .Should().BeTrue();
        }

        [Fact]
        public void ShouldReturnFalseWhenValueIsNotInHeap()
        {
            // Given
            var stringComparer = StringComparer.InvariantCulture;
            var input = "a";

            // When
            var binaryHeap = new BinaryHeap<string>(
                stringComparer
            );

            // Then
            binaryHeap.Contains(input)
                .Should().BeFalse();
        }

        [Fact]
        public void ShouldCountItemsInHeamWhenItemsAreAdded()
        {
            // Given
            var expected = 1;
            var stringComparer = StringComparer.InvariantCulture;
            var input = "a";

            // When
            var binaryHeap = new BinaryHeap<string>(
                stringComparer
            );
            binaryHeap.Add(input);

            // Then
            binaryHeap.Count
                .Should().Be(expected);
        }

        [Fact]
        public void ShouldReturnItemWhenPeekIsUsed()
        {
            // Given
            var expected = "a";
            var stringComparer = StringComparer.InvariantCulture;
            var input = "a";

            // When
            var binaryHeap = new BinaryHeap<string>(
                stringComparer
            );
            binaryHeap.Add(input);
            var actual = binaryHeap.Peek();

            // Then
            actual.Should().Be(expected);
        }

        [Fact]
        public void ShouldNotBeRemovedWhenPeekIsUsed()
        {
            // Given
            var expected = 1;
            var stringComparer = StringComparer.InvariantCulture;
            var input = "a";

            // When
            var binaryHeap = new BinaryHeap<string>(
                stringComparer
            );
            binaryHeap.Add(input);
            var actual = binaryHeap.Peek();

            // Then
            actual.Should().NotBeNull();
            binaryHeap.Count
                .Should().Be(expected);
        }

        [Fact]
        public void ShouldCopyItemsIntoFromTheSpeecifiedIndexWhenTheHeapHasItems()
        {
            // Given
            var expected = new List<string>() { "a", "b" };
            var stringComparer = StringComparer.InvariantCulture;
            var input = new List<string> { "a", "b" }.ToArray();

            // When
            var binaryHeap = new BinaryHeap<string>(
                stringComparer
            );
            foreach (var item in input)
            {
                binaryHeap.Add(item);
            }
            var actual = new string[2];
            binaryHeap.CopyTo(actual, 0);

            // Then
            actual.Should()
                .BeEquivalentTo(expected);
        }

        [Fact]
        public void ShouldRemoveItemsFromHeapWhenRemoveIsCalledWithValidValue()
        {
            // Given
            var input = new List<string>
            {
                "g",
                "k",
                "f",
                "h",
                "i",
                "j",
                "e",
                "d",
                "c",
                "b",
                "a",
            };
            var expectedOrder = new List<string>
            {
                "a",
                "b",
                "c",
                "d",
                "e",
                "i",
                "j",
                "k",
            };
            var valueToRemove1 = "f";
            var valueToRemove2 = "g";
            var valueToRemove3 = "h";
            var stringComparer = StringComparer.InvariantCulture;

            // When
            var binaryHeap = new BinaryHeap<string>(
                stringComparer
            );

            foreach (var stringValue in input)
            {
                binaryHeap.Add(
                    stringValue
                );
            }
            binaryHeap.Remove(
                valueToRemove1
            );
            binaryHeap.Remove(
                valueToRemove2
            );
            binaryHeap.Remove(
                valueToRemove3
            );

            // Then
            foreach (var expected in expectedOrder)
            {
                var actual = binaryHeap.Dequeue();

                actual.Should().Be(expected);
            }
        }

        [Fact]
        public void ShouldBeAbleToUseBinaryHeapAsEnumerator()
        {
            // Given
            var input = new List<string>
            {
                "g",
                "k",
                "f",
                "h",
                "i",
                "j",
                "e",
                "d",
                "c",
                "b",
                "a",
            };
            var expectedOrder = new List<string>
            {
                "a",
                "b",
                "c",
                "d",
                "e",
                "f",
                "g",
                "h",
                "i",
                "j",
                "k",
            };
            var stringComparer = StringComparer.InvariantCulture;

            // When
            var binaryHeap = new BinaryHeap<string>(
                stringComparer
            );

            foreach (var stringValue in input)
            {
                binaryHeap.Add(
                    stringValue
                );
            }

            // Then
            foreach (var actual in binaryHeap)
            {
                expectedOrder.Should().Contain(actual);
            }
        }

        [Fact]
        public void ShouldThrowExceptionWhenCompareIsNullForContructor()
        {
            // Given
            var expected = "comparer";

            // When
            static BinaryHeap<string> action() => new(
                null
            );
            var actual = Assert.Throws<ArgumentNullException>(action);

            // Then
            actual.ParamName
                .Should().Be(expected);
        }

        [Fact]
        public void ShouldReturnTrueWhenIsReadOnlyIsChecked()
        {
            // Given
            var stringComparer = StringComparer.InvariantCulture;

            // When
            var binaryHeap = new BinaryHeap<string>(
                stringComparer
            );
            var actual = binaryHeap.IsReadOnly;

            // Then
            actual.Should().BeFalse();
        }
    }
}
