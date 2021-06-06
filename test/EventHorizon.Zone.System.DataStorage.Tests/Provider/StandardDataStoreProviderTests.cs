namespace EventHorizon.Zone.System.DataStorage.Tests.Provider
{
    using EventHorizon.Zone.System.DataStorage.Model;
    using EventHorizon.Zone.System.DataStorage.Provider;
    using FluentAssertions;
    using global::System.Collections.Generic;
    using Xunit;


    public class StandardDataStoreProviderTests
    {
        [Fact]
        public void ShouldAddNewValueWhenStateDoesNotAlreadyContainValue()
        {
            // Given
            var key = "key";

            var expected = "value";

            // When
            var dataStore = new StandardDataStoreProvider();
            dataStore.TryGetValue<string>(
                key, out _
            ).Should().BeFalse();
            dataStore.AddOrUpdate(
                key,
                expected
            );

            // Then
            dataStore.TryGetValue<string>(
                key,
                out var actual
            ).Should().BeTrue();
            actual.Should().Be(expected);
        }

        [Fact]
        public void ShouldAddUpdateValueWhenStateDoesAlreadyContainValue()
        {
            // Given
            var key = "key";
            var value = "initial-value";

            var expected = "value";

            // When
            var dataStore = new StandardDataStoreProvider();
            dataStore.AddOrUpdate(
                key,
                value
            );
            dataStore.TryGetValue<string>(
                key,
                out var initialValue
            ).Should().BeTrue();
            initialValue
                .Should().Be(value);

            // Add expected value
            dataStore.AddOrUpdate(
                key,
                expected
            );

            // Then
            dataStore.TryGetValue<string>(
                key,
                out var actual
            ).Should().BeTrue();
            actual.Should().Be(expected);
        }

        [Fact]
        public void ShouldReturnDefaultWhenCastIsOfADifferentType()
        {
            // Given
            var key = "key";
            var initialValue = "123";

            var expected = 0;

            // When
            var dataStore = new StandardDataStoreProvider();
            dataStore.AddOrUpdate(
                key,
                initialValue
            );

            // Then
            dataStore.TryGetValue<int>(
                key,
                out var actual
            ).Should().BeFalse();
            actual.Should().Be(expected);
        }

        [Fact]
        public void ShouldReturnNullWhenInvalidCastExceptionCouldNotFixValue()
        {
            // Given
            var key = "key";
            var initialValue = 123;

            var dataStore = new StandardDataStoreProvider();
            dataStore.AddOrUpdate(
                key,
                initialValue
            );

            // Then
            dataStore.TryGetValue<string>(
                key,
                out var actual
            ).Should().BeFalse();
            actual.Should().BeNull();
        }



        [Fact]
        public void ShouldReturnNewCastTypeWhenCastingToTwoSamePropertyTypedTypes()
        {
            // Given
            var key = "key";
            var count = 123;

            var expected = count;

            var initialValue = new TestingInitialStateType
            {
                Count = count,
            };

            var dataStore = new StandardDataStoreProvider();
            dataStore.AddOrUpdate(
                key,
                initialValue
            );

            // Then
            dataStore.TryGetValue<TestingNewStateType>(
                key,
                out var actual
            ).Should().BeTrue();
            actual.Count
                .Should().Be(expected);
        }

        [Fact]
        public void ShouldReturnDictionaryWithKeysWhenDataIsSet()
        {
            // Given
            var input = new Dictionary<string, object>
            {
                ["string"] = "string",
                ["int"] = 123,
                ["complex"] = new
                {
                    Value = "value1",
                },
            };

            // When
            var standardDataStoreProvider = new StandardDataStoreProvider();

            standardDataStoreProvider.Set(
                input
            );

            var actual = standardDataStoreProvider.Data();

            // Then
            actual.Should().ContainKeys(
                "string",
                "int",
                "complex"
            );
        }

        [Fact]
        public void ShouldReturnDictionaryWithValuesWhenDataIsSet()
        {
            // Given
            var input = new Dictionary<string, object>
            {
                ["string"] = "string",
                ["int"] = 123,
                ["complex"] = new
                {
                    Value = "value1",
                },
            };

            // When
            var standardDataStoreProvider = new StandardDataStoreProvider();

            standardDataStoreProvider.Set(
                input
            );

            var actual = standardDataStoreProvider.Data();

            // Then
            actual.Should()
                .ContainKey("string")
                .And
                .Subject["string"]
                    .Should().Be("string");
            actual.Should()
                .ContainKey("int")
                .And
                .Subject["int"]
                    .Should().Be(123);
            actual.Should()
                .ContainKey("complex")
                .And
                .Subject["complex"]
                    .Should().BeEquivalentTo(
                        new
                        {
                            Value = "value1",
                        }
                    );
        }

        [Fact]
        public void ShouldSetKeyAndValueWhenInvidualSetIsCalled()
        {
            // Given
            var key = "key";
            var initialValue = "value1";
            var expected = initialValue;

            // When
            var dataStore = new StandardDataStoreProvider();
            dataStore.Set(
                key,
                initialValue
            );

            // Then
            dataStore.TryGetValue<string>(
                key,
                out var actual
            ).Should().BeTrue();
            actual.Should().Be(expected);
        }

        [Fact]
        public void ShouldRemoveValueWhenDeletedFromProvider()
        {
            // Given
            var key = "key";
            var value = "value1";

            // When
            var dataStore = new StandardDataStoreProvider();
            dataStore.Set(
                key,
                value
            );
            dataStore.TryGetValue<string>(
                key,
                out var existingValue
            ).Should().BeTrue();
            existingValue.Should().Be(value);
            dataStore.Delete(
                key
            );

            // Then
            dataStore.TryGetValue<string>(
                key,
                out var _
            ).Should().BeFalse();
        }

        [Fact]
        public void ShouldUpdateInternalSchemeDetailsWhenKeyTypeIsUpdated()
        {
            // Given
            var key = "key";
            var keyType = "key-type";
            var expected = new DataStoreSchema
            {
                [key] = keyType,
            };

            // When
            var dataStore = new StandardDataStoreProvider();
            dataStore.UpdateSchema(
                key,
                keyType
            );
            dataStore.TryGetValue<DataStoreSchema>(
                StandardDataStoreProvider.DATA_STORE_SCHEMA_KEY,
                out var actual
            ).Should().BeTrue();

            // Then
            actual.Should().BeEquivalentTo(
                expected
            );
        }

        [Fact]
        public void ShouldUpdateExistingInternalSchemeDetailsWhenKeyTypeIsUpdated()
        {
            // Given
            var key = "key";
            var initialType = "key-type";
            var keyType = "update-type";
            var expected = new DataStoreSchema
            {
                [key] = keyType,
            };

            // When
            var dataStore = new StandardDataStoreProvider();
            dataStore.UpdateSchema(
                key,
                initialType 
            );
            dataStore.TryGetValue<DataStoreSchema>(
                StandardDataStoreProvider.DATA_STORE_SCHEMA_KEY,
                out var initialSchema
            ).Should().BeTrue();
            initialSchema.Should().ContainKey(key)
                .And
                .Subject.Should().ContainValue(initialType);
            dataStore.UpdateSchema(
                key,
                keyType
            );
            dataStore.TryGetValue<DataStoreSchema>(
                StandardDataStoreProvider.DATA_STORE_SCHEMA_KEY,
                out var actual
            ).Should().BeTrue();

            // Then
            actual.Should().BeEquivalentTo(
                expected
            );
        }


        [Fact]
        public void ShouldDeleteFromInternalSchemeDetailsWhenKeyDeleteIsExecuted()
        {
            // Given
            var key = "key";
            var expected = new DataStoreSchema();

            // When
            var dataStore = new StandardDataStoreProvider();
            dataStore.DeleteFromSchema(
                key
            );

            // Then
            dataStore.TryGetValue<DataStoreSchema>(
                StandardDataStoreProvider.DATA_STORE_SCHEMA_KEY,
                out var actual
            ).Should().BeTrue();
            actual.Should().BeEquivalentTo(
                expected
            );
        }

        [Fact]
        public void ShouldDeleteFromExistingInternalSchemeDetailsWhenKeyDeleteIsExecuted()
        {
            // Given
            var key = "key";
            var initialType = "key-type";

            // When
            var dataStore = new StandardDataStoreProvider();
            dataStore.UpdateSchema(
                key,
                initialType
            );
            dataStore.TryGetValue<DataStoreSchema>(
                StandardDataStoreProvider.DATA_STORE_SCHEMA_KEY,
                out var initialSchema
            ).Should().BeTrue();
            initialSchema.Should().ContainKey(key);
            dataStore.DeleteFromSchema(
                key
            );

            // Then
            dataStore.TryGetValue<DataStoreSchema>(
                StandardDataStoreProvider.DATA_STORE_SCHEMA_KEY,
                out var actual
            ).Should().BeTrue();
            actual.Should().NotContainKey(key);
        }

        private class TestingInitialStateType
        {
            public int Count { get; set; }
        }

        private class TestingNewStateType
        {
            public int Count { get; set; }
        }
    }
}
