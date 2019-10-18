using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace EventHorizon.Extensions.Tests
{
    public class DictionaryExtensionsTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public DictionaryExtensionsTests(
            ITestOutputHelper testOutputHelper
        )
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void TestShouldReturnThePropertyValueWhenRequestedPropertyName()
        {
            // Given
            var testDataValue = "Vector3DataValue";
            var testData = new Dictionary<string, object>
            {
                { "LongDataValue", 480L },
                { testDataValue, new Vector3(1, 1, 1) }
            };
            var expected = new Vector3(1, 1, 1);

            // When
            var actual = DictionaryExtensions.GetValueOrDefault(
                testData,
                testDataValue,
                Vector3.Zero
            );

            // Then
            Assert.Equal(
                expected,
                actual
            );
        }

        [Fact]
        public void TestShouldReturnDefaultPropertyValueWhenRequestedPropertyNameIsNotFound()
        {
            // Given
            var testDataValue = "Vector3DataValue";
            var testData = new Dictionary<string, object>
            {
                { "LongDataValue", 480L },
            };
            var expected = Vector3.Zero;

            // When
            var actual = DictionaryExtensions.GetValueOrDefault(
                testData,
                testDataValue,
                Vector3.Zero
            );

            // Then
            Assert.Equal(
                expected,
                actual
            );
        }

        [Fact]
        public void TestShouldHaveSpecialRulesForJsonBasedSearializationWhenDeserializeFromString()
        {
            // Given
            var testDataValue = "Vector3DataValue";
            var testDataFromJsonCovert = JsonConvert.DeserializeObject<Dictionary<string, object>>(
                JsonConvert.SerializeObject(
                    new Dictionary<string, object>
                    {
                        {
                            "Vector3DataValue",
                            new Vector3(
                                2,
                                2,
                                2
                            )
                        }
                    }
                )
            );
            var expected = new Vector3(
                2,
                2,
                2
            );

            // When
            var actual = DictionaryExtensions.GetValueOrDefault(
                testDataFromJsonCovert,
                testDataValue,
                Vector3.Zero
            );

            // Then
            Assert.Equal(
                expected,
                actual
            );
        }

        [Theory]
        [Trait("Category", "Performance")]
        [ClassData(typeof(TestDataGenerator))]
        public void TestPerformanceOfGetValueOrDefaultWhenUsingJObject(
            ValueOrDefaultScenarioTestData testData
        )
        {
            // Given
            var testDataFromJsonCovertList = new List<Dictionary<string, object>>();
            for (int i = 0; i < testData.TimesToRun; i++)
            {
                var dataToSerialize = testData.TestData;
                dataToSerialize.Remove("Vector3DataValue");
                dataToSerialize.Add(
                    "Vector3DataValue",
                    new Vector3(
                        i,
                        i,
                        i
                    )
                );
                var testDataAsString = JsonConvert.SerializeObject(
                    dataToSerialize
                );
                var testDataFromJsonCovert = JsonConvert.DeserializeObject<Dictionary<string, object>>(
                    testDataAsString
                );
                testDataFromJsonCovertList.Add(
                    testDataFromJsonCovert
                );
            }
            DictionaryExtensions.GetValueOrDefault<Vector3>(
                testDataFromJsonCovertList.First(),
                "Vector3DataValue",
                Vector3.Zero
            );
            // When
            var watch = Stopwatch.StartNew();
            foreach (var testDataFromJsonCovert in testDataFromJsonCovertList)
            {
                DictionaryExtensions.GetValueOrDefault<Vector3>(
                    testDataFromJsonCovert,
                    "Vector3DataValue",
                    Vector3.Zero
                );
            }

            // Then
            var elapsed = watch.ElapsedMilliseconds;
            _testOutputHelper.WriteLine(
                "{0}",
                testData.TimesToRun
            );
            _testOutputHelper.WriteLine(
                "Time: {0}ms",
                elapsed
            );
            Assert.True(false);
        }

        public class TestDataGenerator : IEnumerable<object[]>
        {
            private readonly List<object[]> _data = new List<object[]>
            {
                new object[] {
                    new ValueOrDefaultScenarioTestData
                    {
                        TimesToRun = 1,
                        TestData = new Dictionary<string, object>
                        {
                            { "LongDataValue", 480L },
                            { "Vector3DataValue", new Vector3(1, 1, 1) }
                        }
                    }
                },
                new object[] {
                    new ValueOrDefaultScenarioTestData
                    {
                        TimesToRun = 1_000,
                        TestData = new Dictionary<string, object>
                        {
                            { "LongDataValue", 480L },
                            { "Vector3DataValue", new Vector3(1, 1, 1) }
                        }
                    }
                },
                new object[] {
                    new ValueOrDefaultScenarioTestData
                    {
                        TimesToRun = 10_000,
                        TestData = new Dictionary<string, object>
                        {
                            { "LongDataValue", 480L },
                            { "Vector3DataValue", new Vector3(1, 1, 1) }
                        }
                    }
                },
                new object[] {
                    new ValueOrDefaultScenarioTestData
                    {
                        TimesToRun = 100_000,
                        TestData = new Dictionary<string, object>
                        {
                            { "LongDataValue", 480L },
                            { "Vector3DataValue", new Vector3(1, 1, 1) }
                        }
                    }
                },
                new object[] {
                    new ValueOrDefaultScenarioTestData
                    {
                        TimesToRun = 1_000_000,
                        TestData = new Dictionary<string, object>
                        {
                            { "LongDataValue", 480L },
                            { "Vector3DataValue", new Vector3(1, 1, 1) }
                        }
                    }
                },
            };

            public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
        public class ValueOrDefaultScenarioTestData
        {
            public long TimesToRun { get; set; }
            public Dictionary<string, object> TestData { get; set; }
        }
    }
}