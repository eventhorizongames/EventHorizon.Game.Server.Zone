using System;
using EventHorizon.Zone.Core.RandomNumber;
using Xunit;

namespace EventHorizon.Zone.Core.Tests.RandomNumber
{
    public class CryptographyRandomNumberGeneratorTests
    {
        [Fact]
        public void TestShouldReturnRandomNumberLessThanOrEqualToMaxValueWhenNextWithMaxValueIsCalled()
        {
            // Given
            var timesToRun = 1_000_000;
            var maxValue = 2030;

            for (int i = 0; i < timesToRun; i++)
            {
                // When
                var randomNumberGenerator = new CryptographyRandomNumberGenerator();
                var actual = randomNumberGenerator.Next(
                    maxValue
                );

                // Then
                Assert.True(
                    actual <= maxValue
                );
            }
        }

        [Fact]
        public void TestShouldReturnRandomNumberBetweenMinAndMaxValueWhenNextWithMinAndMaxValueIsCalled()
        {
            // Given
            var timesToRun = 1_000_000;
            var minValue = 1000;
            var maxValue = 2030;

            for (int i = 0; i < timesToRun; i++)
            {
                // When
                var randomNumberGenerator = new CryptographyRandomNumberGenerator();
                var actual = randomNumberGenerator.Next(
                    minValue,
                    maxValue
                );

                // Then
                Assert.True(
                    actual <= maxValue
                );
                Assert.True(
                    actual >= minValue
                );
            }
        }

        [Fact]
        public void TestShouldReturnRandomNumberBetweenMinValueAndMaxValueWhenNextWithMinAndMaxValueIsCalledWithVerySmallMargin()
        {
            // Given
            var timesToRun = 1_000_000;
            var minValue = 1000;
            var maxValue = 1002;

            for (int i = 0; i < timesToRun; i++)
            {
                // When
                var randomNumberGenerator = new CryptographyRandomNumberGenerator();
                var actual = randomNumberGenerator.Next(
                    minValue,
                    maxValue
                );

                // Then
                Assert.True(
                    actual <= maxValue
                );
                Assert.True(
                    actual >= minValue
                );
            }
        }

        [Fact]
        public void TestShouldThrowArgumentOutOfRangeWhenMinValueIsGreaterThanMaxValue()
        {
            // Given
            var minValue = 1003;
            var maxValue = 1002;

            // When
            var randomNumberGenerator = new CryptographyRandomNumberGenerator();
            Action action = () => randomNumberGenerator.Next(
                minValue,
                maxValue
            );
            
            // Then
            Assert.Throws<ArgumentOutOfRangeException>(
                action
            );
        }
    }
}