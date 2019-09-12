using System.Collections.Generic;
using EventHorizon.Game.I18n.Lookup;
using EventHorizon.Game.I18n.Model;
using Xunit;

namespace EventHorizon.Game.Server.Zone.Tests.I81n.Repository
{
    public class I18nLookupRepositoryTests
    {
        [Fact]
        public void TestLookup_ShouldReturnExpectedRepository()
        {
            //Given
            var locale = "en_US";
            var key = "HelloWorld";
            var keyValue = "hi";

            var expected = new Dictionary<string, string>()
                {{
                    key, keyValue
                }};

            //When
            var i18nLookupRepository = new I18nLookupRepository();
            i18nLookupRepository.SetRepository(
                locale,
                expected
            );

            var actual = i18nLookupRepository.GetRepository(
                locale
            );

            //Then
            Assert.Equal(
                expected,
                actual
            );
        }
        [Fact]
        public void TestLookup_ShouldMergeRepositoriesWhenSameLocaleRepositoriesAreSet()
        {
            //Given
            var locale = "en_US";
            var key1 = "HelloWorld";
            var expectedValue1 = "hi";
            var key2 = "HelloWorld";
            var expectedValue2 = "hi";

            var repository1 = new Dictionary<string, string>()
                {{
                    key1, expectedValue1
                }};
            var repository2 = new Dictionary<string, string>()
                {{
                    key2, expectedValue2
                }};

            //When
            var i18nLookupRepository = new I18nLookupRepository();
            i18nLookupRepository.SetRepository(
                locale,
                repository1
            );
            i18nLookupRepository.SetRepository(
                locale,
                repository2
            );

            var actual = i18nLookupRepository.GetRepository(
                locale
            );

            //Then
            Assert.Equal(
                expectedValue1,
                actual[key1]
            );
            Assert.Equal(
                expectedValue2,
                actual[key2]
            );
        }
        [Fact]
        public void TestSetRepository_ShouldSetEmptyDictionaryOnNullSet()
        {
            //Given
            var locale = "en_US";

            //When
            var i18nLookupRepository = new I18nLookupRepository();
            i18nLookupRepository.SetRepository(
                locale,
                null
            );
            var actual = i18nLookupRepository.GetRepository(
                locale
            );

            //Then
            Assert.Empty(
                actual
            );
        }
        [Fact]
        public void TestLookup_ShouldReturnEmptyRepositoryWhenNotIsFound()
        {
            //Given
            var locale = "en_US";

            //When
            var i18nLookupRepository = new I18nLookupRepository();

            var actual = i18nLookupRepository.GetRepository(
                locale
            );

            //Then
            Assert.Empty(
                actual
            );
        }
        [Fact]
        public void TestLookup_ShouldReturnNotFoundTranslationWhenNotFound()
        {
            //Given
            var expected = "[[HelloWorld (NOT_FOUND)]]";

            var locale = "en_US";
            var key = "HelloWorld";

            //When
            var i18nLookupRepository = new I18nLookupRepository();

            var actual = i18nLookupRepository.Lookup(
                locale,
                key
            );

            //Then
            Assert.Equal(
                expected,
                actual
            );
        }
        [Fact]
        public void TestLookup_ShouldReturnExpectedTranslationWhenFound()
        {
            //Given
            var expected = "Hello, World!";

            var locale = "en_US";
            var key = "HelloWorld";

            //When
            var i18nLookupRepository = new I18nLookupRepository();
            i18nLookupRepository.SetRepository(
                locale,
                new Dictionary<string, string>()
                {{
                    key, expected
                }}
            );

            var actual = i18nLookupRepository.Lookup(
                locale,
                key
            );

            //Then
            Assert.Equal(
                expected,
                actual
            );
        }
        [Fact]
        public void TestLookup_ShouldReturnNotFoundWhenTranslationKeyIsNotFoundInLocaleList()
        {
            //Given
            var expected = "[[HelloWorld (NOT_FOUND)]]";

            var locale = "en_US";
            var key = "HelloWorld";

            //When
            var i18nLookupRepository = new I18nLookupRepository();
            i18nLookupRepository.SetRepository(
                locale,
                new Dictionary<string, string>()
            );

            var actual = i18nLookupRepository.Lookup(
                locale,
                key
            );

            //Then
            Assert.Equal(
                expected,
                actual
            );
        }
        [Fact]
        public void TestLookup_ShouldReturnExpectedResolvedKeyValueTranslationWhenFound()
        {
            //Given
            var expected = "Hello, World!";

            var locale = "en_US";
            var key = "HelloWorld";

            var firstToken = "FIRST";
            var firstValue = "Hello";
            var secondToken = "second";
            var secondValue = "World";

            //When
            var i18nLookupRepository = new I18nLookupRepository();
            i18nLookupRepository.SetRepository(
                locale,
                new Dictionary<string, string>()
                {{
                    key, "${FIRST}, ${second}!"
                }}
            );

            var actual = i18nLookupRepository.Resolve(
                locale,
                key,
                new I18nTokenValue
                {
                    Token = firstToken,
                    Value = firstValue
                },
                new I18nTokenValue
                {
                    Token = secondToken,
                    Value = secondValue
                }
            );

            //Then
            Assert.Equal(
                expected,
                actual
            );
        }
    }
}